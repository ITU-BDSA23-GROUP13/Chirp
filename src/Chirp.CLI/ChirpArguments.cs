using CommandLine;

class ChirpArguments
{
    public class Options
    {
        [Option('r', "read", Required = false, HelpText = "Read Cheeps")]
        public int? CheepCount { get; set; }

        [Option('c', "cheep", Required = false, HelpText = "Cheep a Cheep")]
        public string? CheepMessage { get; set; }
    }
}