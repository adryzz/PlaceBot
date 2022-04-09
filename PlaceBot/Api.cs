using System.Net.Http;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text.Json;

namespace PlaceBot;

public static class Api
{
    public static async Task PlacePixelAsync(Account account, Pixel p)
    {
        if (p.X > 2000 || p.Y > 2000 || p.Color > 31)
        {
            Console.WriteLine($"[{account.Index}] Invalid pixel.");
            return;
        }

        byte[] buffer = new byte[6];
        buffer[0] = 4;
        BitConverter.GetBytes(p.X + p.Y * 2000u).Reverse().ToArray().CopyTo(buffer, 1);
        buffer[5] = p.Color;
        
        await account.Client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
        Console.WriteLine($"[{account.Index}] Pixel placed at ({p.X}, {p.Y}) with color {p.Color} ({Cache.GetColor(p.Color)})");
        //account.Client.PostAsync(PlaceUrl, new StringContent(String.Format(PixelCall, p.X, p.Y, p.Color, getCanvasId(p)))).WaitAsync(CancellationToken.None);
    }

    public static async Task<byte[]> ReceiveMessageAsync(Account account)
    {
        byte[] a = new byte[100];
        var f = new ArraySegment<byte>(a);
        var v = await account.Client.ReceiveAsync(f, CancellationToken.None);
        return a;
    }
}