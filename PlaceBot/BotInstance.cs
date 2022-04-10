namespace PlaceBot;

public class BotInstance
{
    public Account BotAccount;
    
    public BotInstance(Account acc, uint ms = 305*60)
    {
        pixelTimer = new Timer(timerCallback, null, 0, ms);
        BotAccount = acc;
    }

    private Timer pixelTimer;
    
    private async void timerCallback(object? state)
    {
        //randomize pixel timing
        await Task.Delay(Random.Shared.Next(1000, 5000));

        Pixel p = await getNextPixelAsync();

        if (!Program.Options.Force)
        {
            while (Program.Board[p.X, p.Y] == p.Color)
            {
                Console.WriteLine($"[{BotAccount.Index}] Pixel ({p.X}, {p.Y}) is already {Cache.GetColor(p.Color)}. Skipping...");
                p = await getNextPixelAsync();
            }
        }
        
        //place pixel
        await Api.PlacePixelAsync(BotAccount, p);

        await ReceiveDataAsync(await Api.ReceiveMessageAsync(BotAccount));
    }
    
    private async Task<Pixel> getNextPixelAsync()
    {
        lock (Program.Pixels)
        {
            if (Program.Pixels.Count == 0)
            {
                if (Program.Options.Loop)
                {
                    Console.WriteLine("Template done! Restarting...");
                    Program.Pixels = Program.AllPixels;
                }
                else
                {
                    Console.WriteLine("There is nothing left to do.");
                    Environment.Exit(0);
                }
            }
            
            int index = 0;

            if (Program.Options.RandomizePixelPlacementOrder)
                index = Random.Shared.Next(0, Program.Pixels.Count);

            Pixel p = Program.Pixels[index];
            Program.Pixels.RemoveAt(index);
            return p;
        }
    }

    private async Task ReceiveDataAsync(byte[] data)
    {
        switch (data[0])
        {
            case 1:
            {
                Console.WriteLine($"Cooldown: {TimeSpan.FromSeconds(BitConverter.ToUInt32(data, 1) * 1000)}");
                break;
            }
        }
    }
}