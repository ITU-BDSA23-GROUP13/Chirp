using SimpleDB;

namespace IntegrationTestSimpleDb;

public class IntegrationTest
{

    // Records used for testing
    private record IntRecord(int i);
    private record BoolRecord(bool b);
    private record MultiRecord(int i, bool b, string s);

    [Fact]
    public void UnitTestSimpleDBSameFile()
    {
        string fileName = "test_csv";

        var db1 = CsvDatabase<IntRecord>.Instance(fileName);
        var db2 = CsvDatabase<IntRecord>.Instance(fileName);
        var f1 = db1.GetFile().FullName;
        var f2 = db2.GetFile().FullName;

        Assert.Equal(f1, f2);
        Assert.Same(db1, db2);
    }

}
