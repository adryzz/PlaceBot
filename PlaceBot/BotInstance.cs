namespace PlaceBot;

public class BotInstance
{
    public Account BotAccount;
    
    public BotInstance(Account acc, int ms = 305*60)
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
        //place pixel
        await Api.PlacePixelAsync(BotAccount, p);
    }
    
    private async Task<Pixel> getNextPixelAsync()
    {
        lock (Program.Pixels)
        {
            if (!Program.Options.RandomizePixelPlacementOrder) 
                return Program.Pixels.Last();

            return Program.Pixels[Random.Shared.Next(0, Program.Pixels.Count)];
        }
    }
}