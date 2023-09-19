using System;

using CommandLine;

namespace QuickStart
{
    class CommandLine
    {
        public class Options
        {
            [Option('n', "name", Required = true, HelpText = "Your name")]
            public string? Name { get; set; }

            [Option('a', "age", Required = true, HelpText = "Your age")]
            public int Age { get; set; }
        }
    }
}