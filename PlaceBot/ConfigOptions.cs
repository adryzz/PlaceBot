using CommandLine;

namespace PlaceBot;

[Verb("config", HelpText = "Changes the configuration")]
public class ConfigOptions
{
#pragma warning disable CS8618
    
    [Option("useradd", HelpText = "Adds an user with the specified token")]
    public IEnumerable<string> UserAdd { get; set; }
    
#pragma warning restore CS8618
}