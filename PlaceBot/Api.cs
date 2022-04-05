using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace PlaceBot;

public static class Api
{
    public static readonly string PlaceUrl = "https://gql-realtime-2.reddit.com/query";
    public static Task PlacePixelAsync(Account account, Pixel p)
    {
        if (p.X > 2000 || p.Y > 2000 || p.Color > 31)
        {
            Console.WriteLine($"Invalid pixel at account {account.Index}");
            return Task.CompletedTask;
        }
        Console.WriteLine($"[{account.Index}] Pixel placed at ({p.X}, {p.Y}) with color {p.Color} ({Cache.GetColor(p.Color)})");
        //account.Client.PostAsync(PlaceUrl, new StringContent(String.Format(PixelCall, p.X, p.Y, p.Color, getCanvasId(p)))).WaitAsync(CancellationToken.None);
        return Task.CompletedTask;
    }

    public static readonly string PixelCall = "";

    private static int getCanvasId(Pixel p)
    {
        //return (p.X > 1000) + (p.Y > 1000) * 2;
        return 0;
    }
}