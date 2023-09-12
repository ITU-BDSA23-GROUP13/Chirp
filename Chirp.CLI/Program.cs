using SimpleDB;

internal class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static void Main(string[] args)
    {
        IDatabase<Cheep> db = new CSVDatabase<Cheep>();

        if (args.Length >= 1)
        {
            if (args[0] == "read")
            {
                IEnumerable<Cheep> cheeps;

                if (args.Length >= 2 && int.TryParse(args[1], out int count))
                {
                    cheeps = db.Read(count);
                }
                else
                {
                    cheeps = db.Read();
                }

                foreach (Cheep cheep in cheeps)
                {
                    string author = cheep.Author;
                    string message = cheep.Message;
                    DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToLocalTime();

                    Console.WriteLine($"{author} @ {date:MM\\/dd\\/yy HH:mm:ss}: {message}");
                }
            }
            else if (args[0] == "cheep")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Use argument: 'cheep <message>'");
                    return;
                }

                string Author = Environment.UserName;
                string Message = args[1];
                long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                db.Store(new Cheep(Author, Message, Timestamp));
            }
        }
        else
        {
            Console.WriteLine("Use argument: 'read' or 'cheep'");
            return;
        }
    }
}
