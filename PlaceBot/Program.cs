using System.Text.Json;
using CommandLine;

namespace PlaceBot;

public static class Program
{
    public static List<Pixel> AllPixels;
    
    public static List<Pixel> Pixels;

    public static byte[,] Board;

    public static BotOptions Options;
    
    public static List<BotInstance> Instances = new List<BotInstance>();

    private static List<Account> Accounts = new List<Account>();
    
    public static async Task Main(string[] args)
    {
        try
        {
            Parsed<object> o = (Parsed<object>) Parser.Default.ParseArguments<BotOptions, ConfigOptions>(args);

            if (o.Value is ConfigOptions c)
            {
                await configureAsync(c);
                return;
            }

            if (o.Value is not BotOptions b)
                return;

            Options = b;
        }
        catch
        {
            return;
        }

        AllPixels = await Cache.LoadPixelsAsync(Options.TemplatePath, Options.RegenerateCaches);

        if (Options.VerticalPixelPlacementOrder)
            AllPixels = AllPixels.OrderBy(x => x.X).ThenBy(y => y.Y).ToList();
        
        Pixels = AllPixels;
        
        Board = new byte[2000, 2000];

        for (int x = 0; x < 2000; x++)
        {
            for (int y = 0; y < 2000; y++)
            {
                Board[x, y] = Byte.MaxValue; //set it to an illegal color when starting
            }
        }

        if (!File.Exists("accounts.json"))
        {
            await Console.Error.WriteLineAsync("No accounts found. Make sure you have added all the accounts to 'accounts.json'");
            return;
        }

        using Stream acc = File.OpenRead("accounts.json");
        Accounts = await JsonSerializer.DeserializeAsync<List<Account>>(acc);

        if (Accounts.Count == 0)
        {
            await Console.Error.WriteLineAsync("No accounts found. Make sure you have added all the accounts to 'accounts.json'");
            return;
        }
        Console.WriteLine($"Found {Accounts.Count} accounts.");
        
        foreach (var a in Accounts)
        {
            await a.InitializeAsync();
            Instances.Add(new BotInstance(a, Options.PixelCooldown));
            
            Console.WriteLine($"[{a.Index}] Account started!");
        }

        await Task.Delay(-1);
    }

    private static async Task configureAsync(ConfigOptions options)
    {
        List<Account> a = new List<Account>();
        if (File.Exists("accounts.json"))
        {
            using (Stream acc = File.OpenRead("accounts.json"))
            {
                a = await JsonSerializer.DeserializeAsync<List<Account>>(acc);
            }
        }

        int max = a.Count + options.UserAdd.Count();
        
        for (int i = a.Count; i < max; i++)
        {
            a.Add(new Account(options.UserAdd.ElementAt(i)) {Index = i});
        }

        using (Stream acc = File.OpenWrite("accounts.json"))
        {
            await JsonSerializer.SerializeAsync(acc, a);
        }
    }
}