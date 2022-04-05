using System.Diagnostics;
using System.Security.Cryptography;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PlaceBot;

public static class Cache
{
    public enum Colors : uint
    {
        Burgundy = 0x6D001AFF,
        DarkRed = 0xBE0039FF,
        Red = 0xFF4500FF,
        Orange = 0xFFA800FF,
        Yellow = 0xFFD635FF,
        PaleYellow = 0xFFF8B8FF,
        DarkGreen = 0x00A368FF,
        Green = 0x00CC78FF,
        LightGreen = 0x7EED56FF,
        DarkTeal = 0x00756FFF,
        Teal = 0x009EAAFF,
        LightTeal = 0x00CCC0FF,
        DarkBlue = 0x2450A4FF,
        Blue = 0x3690EAFF,
        LightBlue = 0x51E9F4FF,
        Indigo = 0x493AC1FF,
        Periwinkle = 0x6A5CFFFF,
        Lavender = 0x94B3FFFF,
        DarkPurple = 0x811E9F,
        Purple = 0xB44AC0FF,
        PalePurple = 0xE4ABFFFF,
        Magenta = 0xDE107FFF,
        Pink = 0xFF3881FF,
        LightPink = 0xFF99AAFF,
        DarkBrown = 0x6D482FFF,
        Brown = 0x9C6926FF,
        Beige = 0xFFB470FF,
        Black = 0x000000FF,
        DarkGray = 0x515252FF,
        Gray = 0x898D90FF,
        LightGray = 0xD4D7D9FF,
        White = 0xFFFFFFFF
    }

    public static async Task<List<Pixel>> GenerateCacheAsync(string template = "template.png")
    {
        List<Pixel> pixels = new List<Pixel>();

        byte[] img = await File.ReadAllBytesAsync(template);
        byte[] hash = SHA1.HashData(img);

        Stream cache = new BufferedStream(File.OpenWrite("cache.bin"));
        cache.Position = 0;

        //write the template hash
        await cache.WriteAsync(hash);

        using Image<Rgba32> image = await Image.LoadAsync<Rgba32>(new MemoryStream(img));

        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                // pixelRow.Length has the same value as accessor.Width,
                // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                for (int x = 0; x < pixelRow.Length; x++)
                {
                    // Get a reference to the pixel at position x
                    ref Rgba32 pixel = ref pixelRow[x];
                    if (Enum.IsDefined(typeof(Colors), pixel.PackedValue))
                        pixels.Add(new Pixel(x, y, GetColor((Colors)pixel.PackedValue)));
                }
            }
        });

        //save cache
        foreach (Pixel p in pixels)
        {
            await cache.WriteAsync(BitConverter.GetBytes(p.X));
            await cache.WriteAsync(BitConverter.GetBytes(p.Y));
            await cache.WriteAsync(BitConverter.GetBytes(p.Color));
        }

        await cache.DisposeAsync();

        return pixels;
    }

    public static async Task<List<Pixel>> LoadPixelsAsync(string template = "template.png")
    {
        if (!File.Exists("cache.bin"))
        {
            return await GenerateCacheAsync(template);
        }

        byte[] img = await File.ReadAllBytesAsync(template);
        FileStream cache = File.OpenRead(template);

        byte[] hash = new byte[20];
        await cache.ReadAsync(hash);

        byte[] hash1 = SHA1.HashData(img);

        if (!hash.SequenceEqual(hash1))
        {
            await cache.DisposeAsync();
            return await GenerateCacheAsync();
        }

        //use capacity just because we can and it speeds up
        List<Pixel> pixels = new List<Pixel>((int) ((cache.Length - 20) / 9));

        byte[] buffer = new byte[9];
        for (int i = 0; i < pixels.Capacity; i++)
        {
            if (await cache.ReadAsync(buffer) != 9)
                break;

            pixels.Add(new Pixel(BitConverter.ToInt32(buffer), BitConverter.ToInt32(buffer, 4), buffer[8]));
        }

        await cache.DisposeAsync();
        return pixels;
    }
    
    public static byte GetColor(Colors color) => color switch
    {
        Colors.Burgundy => 0,
        Colors.DarkRed => 1,
        Colors.Red => 2,
        Colors.Orange => 3,
        Colors.Yellow => 4,
        Colors.PaleYellow => 5,
        Colors.DarkGreen => 6,
        Colors.Green => 7,
        Colors.LightGreen => 8,
        Colors.DarkTeal => 9,
        Colors.Teal => 10,
        Colors.LightTeal => 11,
        Colors.DarkBlue => 12,
        Colors.Blue => 13,
        Colors.LightBlue => 14,
        Colors.Indigo => 15,
        Colors.Periwinkle => 15,
        Colors.Lavender => 16,
        Colors.DarkPurple => 17,
        Colors.Purple => 18,
        Colors.PalePurple => 19,
        Colors.Magenta => 20,
        Colors.Pink => 21,
        Colors.LightPink => 23,
        Colors.DarkBrown => 24,
        Colors.Brown => 25,
        Colors.Beige => 26,
        Colors.Black => 27,
        Colors.DarkGray => 28,
        Colors.Gray => 29,
        Colors.LightGray => 30,
        Colors.White => 31
    };
    
    public static Colors GetColor(byte color) => color switch
    {
        0 => Colors.Burgundy,
        1 => Colors.DarkRed,
        2 => Colors.Red,
        3 => Colors.Orange,
        4 => Colors.Yellow,
        5 => Colors.PaleYellow,
        6 => Colors.DarkGreen,
        7 => Colors.Green,
        8 => Colors.LightGreen,
        9 => Colors.DarkTeal,
        10 => Colors.Teal,
        11 => Colors.LightTeal,
        12 => Colors.DarkBlue,
        13 => Colors.Blue,
        14 => Colors.LightBlue,
        15 => Colors.Indigo,
        16 => Colors.Periwinkle,
        17 => Colors.Lavender,
        18 => Colors.DarkPurple,
        19 => Colors.Purple,
        20 => Colors.PalePurple,
        21 => Colors.Magenta,
        22 => Colors.Pink,
        23 => Colors.LightPink,
        24 => Colors.DarkBrown,
        25 => Colors.Brown,
        26 => Colors.Beige,
        27 => Colors.Black,
        28 => Colors.DarkGray,
        29 => Colors.Gray,
        30 => Colors.LightGray,
        31 => Colors.White
    };
}