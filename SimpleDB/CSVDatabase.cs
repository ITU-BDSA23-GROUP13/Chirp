using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabase<T>
{
    public IEnumerable<T> Read(int limit = int.MaxValue) {
        using (var reader = new StreamReader(".\\testdb.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<T>().Take(limit).ToList();
        }
    }

    public void Store(T record) {
        throw new NotImplementedException();
    }
}


