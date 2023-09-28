using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CsvDatabase<T> : IDatabase<T>
{
    // Default directory and extension for CSV files.
    private const string dir = "./csvdata/";
    private const string ext = ".csv";

    private static CsvConfiguration defaultConfig;
    private static readonly Dictionary<string, CsvDatabase<T>> instances;

    private readonly FileInfo file;

    static CsvDatabase()
    {
        defaultConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
        defaultConfig.InjectionOptions = InjectionOptions.Escape;
        instances = new Dictionary<string, CsvDatabase<T>>();
    }

    /// <summary> Use CSVDatabase.Instance(file) instead. </summary>
    private CsvDatabase(FileInfo file)
    {
        this.file = file;
    }

    public static CsvDatabase<T> Instance(FileInfo file)
    {
        string filePath = file.FullName;

        if (instances.TryGetValue(filePath, out CsvDatabase<T>? db))
        {
            return db;
        }
        else
        {
            db = new CsvDatabase<T>(file);
            instances.Add(filePath, db);
            return db;
        }
    }

    public static CsvDatabase<T> Instance(string fileName)
    {
        FileInfo file = new FileInfo(dir + fileName + ext);

        return CsvDatabase<T>.Instance(file);
    }

    public IEnumerable<T> Read(int limit = int.MaxValue)
    {
        if (!file.Exists) return Enumerable.Empty<T>();

        using (var reader = new StreamReader(file.FullName))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().Take(limit).ToList();
        }
    }

    public void Store(T record)
    {
        if (!file.Exists)
        {
            file.Directory?.Create();
            file.Create().Flush(); // NOTE: Might be unnecessary?
        }

        using (var writer = File.AppendText(file.FullName))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord<T>(record);
            writer.WriteLine();
            writer.Flush();
        }
    }

    public FileInfo GetFile() => file;
}