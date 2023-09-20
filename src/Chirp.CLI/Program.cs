using SimpleDB;

public class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLine.Options>(args)
        .WithParsed(options =>
        {
            IDatabase db = CSVDatabase.Instance();
            //Read cheeps
            if (options.CheepCount != null)
            {
                var cheeps = db.Read<Cheep>(options.CheepCount.Value);
                UserInterface.PrintCheeps(cheeps);
            }

            //Cheep a cheep
            if (!string.IsNullOrWhiteSpace(options.CheepMessage))
            {
                string Author = Environment.UserName;
                string Message = options.CheepMessage;
                long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                db.Store<Cheep>(new Cheep(Author, Message, Timestamp));

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