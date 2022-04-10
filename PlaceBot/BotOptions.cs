using CommandLine;

namespace PlaceBot;

[Verb("run", true)]
public class BotOptions
{
#pragma warning disable CS8618
    
    [Option('t', "template", HelpText = "The template to draw on the canvas.", Default = "template.png")]
    public string TemplatePath { get; set; }
    
    [Option('r', "random", HelpText = "Whether or not to randomize pixel placement order", Default = false, SetName = "random")]
    public bool RandomizePixelPlacementOrder { get; set; }
    
    [Option('v', "vertical", HelpText = "Place pixels in vertical order", Default = false, SetName = "vertical")]
    public bool VerticalPixelPlacementOrder { get; set; }
    
    [Option('c', "cooldown", HelpText = "Pixel placement cooldown (ms)", Default = 305u*60u)]
    public uint PixelCooldown { get; set; }
    
    [Option('l', "loop", HelpText = "Restarts the template when done.", Default = false)]
    public bool Loop { get; set; }
    
    [Option('f', "force", HelpText = "Places a pixel even if the pixel already has that color.", Default = false)]
    public bool Force { get; set; }
    
    [Option("regenerate-cache", HelpText = "Regenerates caches", Default = false)]
    public bool RegenerateCaches { get; set; }
    
#pragma warning restore CS8618
}