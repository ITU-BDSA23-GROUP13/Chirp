using SimpleDB;
using CommandLine;

namespace Chirp.CLI;

public class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<ChirpArguments.Options>(args)
        .WithParsed(options =>
        {
            IDatabase<Cheep> db = CsvDatabase<Cheep>.Instance("cheeps");

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
    }
}