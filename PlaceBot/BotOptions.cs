using CommandLine;

namespace PlaceBot;

[Verb("run", true)]
public class BotOptions
{
#pragma warning disable CS8618
    
    [Option('t', "template", HelpText = "The template to draw on the canvas.", Default = "template.png")]
    public string TemplatePath { get; set; }
    
    [Option('r', "random", HelpText = "Whether or not to randomize pixel placement order", Default = false)]
    public bool RandomizePixelPlacementOrder { get; set; }
    
    [Option('c', "cooldown", HelpText = "Pixel placement cooldown (seconds)", Default = 305*60)]
    public uint PixelCooldown { get; set; }
    
#pragma warning restore CS8618
}