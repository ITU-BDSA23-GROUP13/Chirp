using SimpleDB;
using QuickStart;
using CommandLine;


public class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<QuickStart.CommandLine.Options>(args)
            .WithParsed(options =>
            {
                IDatabase<Cheep> db = new CSVDatabase<Cheep>();
                //Read cheeps
                if (options.CheepCount != null)
                {

                    var cheeps = db.Read(options.CheepCount.Value);
                    UserInterface.PrintCheeps(cheeps);
                }

                //Cheep a cheep
                if (!string.IsNullOrWhiteSpace(options.CheepMessage))
                {

                    string Author = Environment.UserName;
                    string Message = options.CheepMessage;
                    long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    db.Store(new Cheep(Author, Message, Timestamp));

                    UserInterface.PrintMessage($"Cheeped a cheep! The cheep is: {options.CheepMessage}");
                }

            })
            .WithNotParsed(errors =>
            {
                // Handle parsing errors if any
                Console.WriteLine("Invalid command-line arguments.");
            });




        /*

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

                        UserInterface.PrintCheeps(cheeps);
                        return;
                    }
                    else if (args[0] == "cheep")
                    {
                        if (args.Length < 2)
                        {
                            UserInterface.PrintMessage("Use argument: 'cheep <MESSAGE>'");
                            return;
                        }

                        string Author = Environment.UserName;
                        string Message = args[1];
                        long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                        db.Store(new Cheep(Author, Message, Timestamp));
                        return;
                    }
                }
                UserInterface.PrintMessage("Use argument: 'read <COUNT>' or 'cheep <MESSAGE>'");
                return;
                */
    }
}