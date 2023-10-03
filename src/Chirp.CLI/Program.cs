using CommandLine;
using System.Net;
using System.Net.Http.Json;

namespace Chirp.CLI;

public static class Program
{
    public record Cheep(string Author, string Message, long Timestamp)
    {
        public DateTime TimestampAsDateTime
            => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;
    }

    private const string serverBaseAddress = "https://bdsagroup13chirpremotedb.azurewebsites.net";
    private const string serverReadAddress = "/cheeps";
    private const string serverStoreAddress = "/cheep";

    private static void Main(string[] args)
    {
        /*
         * NOTE: async functions does not seem to work with CommandLineParser,
         * so we run the networking synchronously, which is okay because there
         * is not other work to do in the meantime anyway.
         */

        Parser.Default.ParseArguments<ChirpArguments.Options>(args)
        .WithParsedAsync(async (ChirpArguments.Options options) =>
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(serverBaseAddress);

            //Read cheeps
            if (options.CheepCount is not null)
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(serverReadAddress);
                }
                catch (Exception e)
                {
                    UserInterface.PrintMessage(e.Message);
                    return;
                }

                if (response.StatusCode is not HttpStatusCode.OK)
                {
                    UserInterface.PrintMessage(response.StatusCode switch {
                        HttpStatusCode.NotFound => "Page not found.",
                        HttpStatusCode.InternalServerError => "Internal server error.",
                        _ => "Failed to read cheeps from server. Status code: " + response.StatusCode,
                    });
                    return;
                }

                var cheeps = await response.Content.ReadFromJsonAsync<List<Cheep>>();

                if (cheeps is null)
                {
                    UserInterface.PrintMessage("Failed to parse cheeps from server.");
                    return;
                }
                if (cheeps.Count == 0)
                {
                    UserInterface.PrintMessage("There are no cheeps.");
                    return;
                }
                cheeps.Sort((a, b) => DateTime.Compare(a.TimestampAsDateTime, b.TimestampAsDateTime));

                UserInterface.PrintCheeps(cheeps.Take((int) options.CheepCount));
            }

            //Cheep a cheep
            if (!string.IsNullOrWhiteSpace(options.CheepMessage))
            {
                string Author = Environment.UserName;
                string Message = options.CheepMessage;
                long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                try
                {
                    var cheep = new Cheep(Author, Message, Timestamp);
                    var response = await client.PostAsJsonAsync(serverStoreAddress, cheep);

                    if (response.StatusCode is not HttpStatusCode.OK)
                    {
                        UserInterface.PrintMessage(response.StatusCode switch {
                            HttpStatusCode.NotFound => "Page not found.",
                            HttpStatusCode.InternalServerError => "Internal server error.",
                            _ => "Failed to cheep to server. Status code: " + response.StatusCode,
                        });
                        UserInterface.PrintMessage(response.ToString());
                        return;
                    }

                    //UserInterface.PrintMessage($"Cheeped a cheep! The cheep is: {options.CheepMessage}");
                }
                catch (Exception e)
                {
                    UserInterface.PrintMessage(e.Message);
                    return;
                }
            }
        }).Result
        .WithNotParsed(errors =>
        {
            // Handle parsing errors if any
            Console.WriteLine("Invalid command-line arguments.");
        });
    }
}