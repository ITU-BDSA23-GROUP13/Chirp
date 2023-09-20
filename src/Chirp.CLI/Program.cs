using SimpleDB;

public class Program
{
    public record Cheep(string Author, string Message, long Timestamp);
    
    private static void Main(string[] args)
    {
        IDatabase db = CSVDatabase.Instance();

        if (args.Length >= 1)
        {
            if (args[0] == "read")
            {
                IEnumerable<Cheep> cheeps;

                if (args.Length >= 2 && int.TryParse(args[1], out int count))
                {
                    cheeps = db.Read<Cheep>(count);
                }
                else
                {
                    cheeps = db.Read<Cheep>();
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

                db.Store<Cheep>(new Cheep(Author, Message, Timestamp));
                return;
            }
        }
        UserInterface.PrintMessage("Use argument: 'read <COUNT>' or 'cheep <MESSAGE>'");
        return;
    }
}