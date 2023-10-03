using SimpleDB;
using CommandLine;
using System.Net;
using System.Net.Http.Json;

namespace Chirp.CLI;

public class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private const string serverAddress = "https://bdsagroup13chirpremotedb.azurewebsites.net";

    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<ChirpArguments.Options>(args)
        .WithParsed(async (ChirpArguments.Options options) =>
        {
            IDatabase<Cheep> db = CsvDatabase<Cheep>.Instance("cheeps");

            //Read cheeps
            if (options.CheepCount != null)
            {
                //var cheeps = db.Read(options.CheepCount.Value);
                var client = new HttpClient();
                client.BaseAddress = new Uri(serverAddress);

                HttpResponseMessage response;
                try {
                    response = await client.GetAsync("/");
                }
                catch (Exception e)
                {
                    UserInterface.PrintMessage(e.Message);
                    return;
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    UserInterface.PrintMessage("Failed to connect to server.");
                    return;
                }

                var cheeps = await System.Net.Http.Json.HttpContentJsonExtensions.ReadFromJsonAsync<List<Cheep>>(response.Content);
                if (cheeps is null)
                {
                    UserInterface.PrintMessage("Failed to read cheeps from server.");
                    return;
                }
                if (cheeps.Count == 0)
                {
                    UserInterface.PrintMessage("No cheeps found.");
                    return;
                }

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