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
            command.CommandText =
            @"
            SELECT username
            FROM user
            WHERE user_id = 5
            ";

            using var reader = command.ExecuteReader();
            while(reader.Read())
            {
                Object[] values = new Object[reader.FieldCount];
                reader.GetValues(values);

                Console.WriteLine(values[0]);
            }

            connection.Close();
        }
    }
}