using static Program;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (Cheep cheep in cheeps)
        {
            string author = cheep.Author;
            string message = cheep.Message;
            DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToLocalTime();

            Console.WriteLine($"{author} @ {date:MM\\/dd\\/yy HH:mm:ss}: {message}");
        }
    }

    public static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }

}