using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CsvDatabase<T> : IDatabase<T>
{
    private const string dir = "./csvdata/";
    private const string ext = ".csv";

    private static CsvConfiguration defaultConfig;
    private static readonly Dictionary<FileInfo, CsvDatabase<T>> instances;

    private readonly FileInfo file;

    static CsvDatabase()
    {
        defaultConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
        defaultConfig.InjectionOptions = InjectionOptions.Escape;
        instances = new Dictionary<FileInfo, CsvDatabase<T>>();
    }

    /// <summary> Use CSVDatabase.Instance() instead. </summary>
    private CsvDatabase(string fileName)
    {
        this.file = getFileInfo(fileName);
    }

    public static CsvDatabase<T> Instance(string fileName)
    {
        FileInfo file = getFileInfo(fileName);

        if (instances.TryGetValue(file, out CsvDatabase<T>? db))
        {
            return db;
        }
        else
        {
            db = new CsvDatabase<T>(fileName);
            instances.Add(file, db);
            return db;
        }
    }

    public IEnumerable<T> Read(int limit = int.MaxValue)
    {
        ensureFileExists(file.FullName);
        using (var reader = new StreamReader(file.FullName))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().Take(limit).ToList();
        }
    }

    public void Store(T record)
    {
        ensureFileExists(file.FullName);
        using (var writer = File.AppendText(file.FullName))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord<T>(record);
            writer.WriteLine();
            writer.Flush();
        }
    }

    private static FileInfo getFileInfo(string fileName)
    {
        return new FileInfo(dir + fileName + ext);
    }

    private static void ensureFileExists(string path)
    {
        if (!File.Exists(path))
        {
            File.Create(path);
        }
    }
}