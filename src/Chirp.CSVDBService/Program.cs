using SimpleDB;

namespace Chirp.CsvDbService;

public class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static readonly IDatabase<Cheep> db = CsvDatabase<Cheep>.Instance("cheeps");

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Hello World! We are testing right now :)");
        app.MapGet("/cheeps", Read);
        app.MapPost("/cheep/", Store);
        app.MapPost("/cheep/{author}/{message}", StoreNew);
        app.MapDelete("/cheep/", Delete);

        app.Run();
    }

    private static String Read()
    {
        var cheeps = db.Read() ?? Enumerable.Empty<Cheep>();
        var jsonString = System.Text.Json.JsonSerializer.Serialize(cheeps.ToList());
        return jsonString;
    }

    private static void Store(Cheep cheep)
    {
        db.Store(cheep);
    }

    private static void StoreNew(string author, string message)
    {
        db.Store(new Cheep(author, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
    }

    private static void Delete()
    {
        db.DeleteAll();
    }
}