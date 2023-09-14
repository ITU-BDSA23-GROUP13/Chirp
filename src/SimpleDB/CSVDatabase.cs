using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabase<T>
{
    private const string path = ".\\testdb.csv";

    public IEnumerable<T> Read(int limit = int.MaxValue) {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().Take(limit).ToList();
        }
    }

    public void Store(T record) {
        using (var writer = File.AppendText(path))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
            writer.Flush();
        }
    }
}


