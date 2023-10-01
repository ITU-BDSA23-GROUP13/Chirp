using SimpleDB;

namespace Chirp.CsvDbService;

public class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        IDatabase<Cheep> db = CsvDatabase<Cheep>.Instance("cheeps");

        app.MapGet("/", () => "Hello World!");
        app.MapGet("/cheeps", () => db.Read().Select(cheep => cheep.ToString()).Aggregate((a, b) => $"{a}\n{b}"));
        app.MapPost("/cheep", (Cheep cheep) => db.Store(cheep));

        app.Run();
    }
}