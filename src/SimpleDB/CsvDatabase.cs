﻿using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CsvDatabase<T> : IDatabase<T>
{
    // Default directory and extension for CSV files.
    private const string dir = "./csvdata/";
    private const string ext = ".csv";

    private static CsvConfiguration defaultConfig;
    /// <summary> Maps file paths to database instances. </summary>
    private static readonly Dictionary<string, CsvDatabase<T>> instances;

    private readonly FileInfo file;
    private readonly CsvConfiguration config;

    static CsvDatabase()
    {
        defaultConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
        defaultConfig.HasHeaderRecord = false;
        defaultConfig.InjectionOptions = InjectionOptions.Escape;
        instances = new Dictionary<string, CsvDatabase<T>>();
    }

    /// <summary> Use CSVDatabase.Instance(file) instead. </summary>
    private CsvDatabase(FileInfo file, CsvConfiguration? config = null)
    {
        this.file = file;
        this.config = config ?? defaultConfig;
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

    public IEnumerable<T> Read(int count = int.MaxValue)
    {
        if (!file.Exists) return Enumerable.Empty<T>();

        using (var reader = new StreamReader(file.FullName))
        using (var csv = new CsvReader(reader, defaultConfig))
        {
            return csv.GetRecords<T>().Take(count).ToList();
        }
    }

    public void Store(T record)
    {
        if (!file.Exists)
        {
            // Create directory. The file itself is created by the StreamWriter if missing.
            file.Directory?.Create();
        }

        const bool shouldAppend = true;
        using (var writer = new StreamWriter(file.FullName, shouldAppend))
        using (var csv = new CsvWriter(writer, defaultConfig))
        {
            csv.WriteRecord<T>(record);
            writer.WriteLine();
            writer.Flush();
        }
    }

    public void Store(IEnumerable<T> records)
    {
        if (!file.Exists)
        {
            file.Directory?.Create();
        }

        const bool shouldAppend = true;
        using (var writer = new StreamWriter(file.FullName, shouldAppend))
        using (var csv = new CsvWriter(writer, defaultConfig))
        {
            csv.WriteRecords<T>(records);
            writer.WriteLine();
            writer.Flush();
        }
    }

    public void DeleteAll()
    {
        if (!file.Exists) return;

        file.Delete();

        using (var newFile = file.Create())
        {
            newFile.Flush();
        }
    }

    public FileInfo GetFile() => file;
}