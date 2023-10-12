using System.Globalization;

using static Chirp.CLI.Program;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (Cheep cheep in cheeps)
        {
            string author = cheep.Author;
            string message = cheep.Message;
            DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp);

            // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings?redirectedfrom=MSDN
            Console.WriteLine($"{author} @ {date.ToString(@"MM\/dd\/yy HH:mm:ss", CultureInfo.InvariantCulture)}: {message}");
        }
    }

    public static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }
}