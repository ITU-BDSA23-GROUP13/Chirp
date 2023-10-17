using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade
{
    public static void RunDB()
    {
        var sqliteBuilder = new SqliteConnectionStringBuilder{
            DataSource = Path.Combine(Path.GetTempPath(), "chirp.db"),
        };
        var sqlDBFilePath = sqliteBuilder.ToString();
        
        using (var connection = new SqliteConnection(sqlDBFilePath))
        {
            connection.Open();

            var command = connection.CreateCommand();
            

            using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Assuming there's only one result row
                        string username = reader.GetString(0);
                        Console.WriteLine($"Username: {username}");
                    }
                }
            

            connection.Close();
        }
    }
}