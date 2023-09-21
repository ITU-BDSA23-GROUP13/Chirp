using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CSVDatabase : IDatabase
{
    private const string dir = "./csvdata/";
    private const string ext = ".csv";
    private static CsvConfiguration config;
    private static CSVDatabase instance = new CSVDatabase();

    static CSVDatabase()
    {
        config = new CsvConfiguration(CultureInfo.InvariantCulture);
        config.InjectionOptions = InjectionOptions.Escape;
    }

    /// <summary> Use CSVDatabase.Instance() instead. </summary>
    private CSVDatabase() {}

    public static CSVDatabase Instance()
        => instance;

    public IEnumerable<T> Read<T>(int limit = int.MaxValue)
    {
        string path = getPath<T>();
        ensureFileExists(path);
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().Take(limit).ToList();
        }
    }

    public void Store<T>(T record)
    {
        string path = getPath<T>();
        ensureFileExists(path);
        using (var writer = File.AppendText(path))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord<T>(record);
            writer.WriteLine();
            writer.Flush();
        }
    }

    private static string getPath<T>()
    {
        string fileName = "test";
        return dir + fileName + ext;
    }

    private static void ensureFileExists(string path)
    {
        if (!File.Exists(path))
        {
            File.Create(path);
        }
    }
}