using CommandLine;

namespace Chirp.CLI;

[Verb("read", HelpText = "Read the given amount of Cheeps.")]
public class ReadOptions
{
    [Value(0, Default = null, HelpText = "Amount of cheeps read. (Default: all)")]
    public int? CheepCount { get; set; }
}

[Verb("cheep", HelpText = "Cheep a Cheep with the given message.")]
public class CheepOptions
{
    [Value(0, Required = true, HelpText = "Message to Cheep. (Required)")]
    public required string CheepMessage { get; set; }
}