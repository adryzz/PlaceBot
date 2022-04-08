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
        //place pixel
        await Api.PlacePixelAsync(BotAccount, p);
    }
    
    private async Task<Pixel> getNextPixelAsync()
    {
        lock (Program.Pixels)
        {
            int index = Program.Pixels.Count - 1;

            if (Program.Options.RandomizePixelPlacementOrder)
                index = Random.Shared.Next(0, Program.Pixels.Count);

            Pixel p = Program.Pixels[index];
            Program.Pixels.RemoveAt(index);
            return p;
        }
    }
}