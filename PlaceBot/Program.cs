using CommandLine;

namespace PlaceBot;

public static class Program
{
    public static List<Pixel> AllPixels;
    
    public static List<Pixel> Pixels;

    public static Options Options;
    
    public static async Task Main(string[] args)
    {
        Options = Parser.Default.ParseArguments<Options>(args).Value;
        
        AllPixels = await Cache.LoadPixelsAsync(Options.TemplatePath);
        
        Pixels = AllPixels;
        List<BotInstance> instances = new List<BotInstance>();
        for (uint i = 0; i < 500000; i++)
        {
            instances.Add(new BotInstance(new Account("cock") {Index = i}));
        }

        await Task.Delay(-1);
    }
}