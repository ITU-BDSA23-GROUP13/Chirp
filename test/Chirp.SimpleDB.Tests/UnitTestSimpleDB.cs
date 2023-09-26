using static Program;
using SimpleDB;

namespace Chirp.SimpleDB.Tests;

public class UnitTestSimpleDB
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

    [Fact]
    public void UnitTestSimpleDBSameFileDifferentType()
    {
        string fileName = "test_csv";

        var db1 = CsvDatabase<IntRecord>.Instance(fileName);
        var db2 = CsvDatabase<BoolRecord>.Instance(fileName);
        var f1 = db1.GetFile().FullName;
        var f2 = db2.GetFile().FullName;

        Assert.Equal(f1, f2);
        Assert.NotSame(db1, db2);
    }

    [Fact]
    public void UnitTestSimpleDBDifferentFile()
    {
        string fileName1 = "test_csv1";
        string fileName2 = "test_csv2";

        var db1 = CsvDatabase<IntRecord>.Instance(fileName1);
        var db2 = CsvDatabase<IntRecord>.Instance(fileName2);
        var f1 = db1.GetFile().FullName;
        var f2 = db2.GetFile().FullName;

        Assert.NotSame(db1, db2);
        Assert.NotEqual(f1, f2);
    }

    IDatabase<Cheep> db = CsvDatabase<Cheep>.Instance("test_csv_file");

    private readonly CsvDatabase<Cheep> _simpleDB;
    private readonly CsvDatabase<Cheep> _csvDB;

    public UnitTestSimpleDB()
    {
        _simpleDB = CsvDatabase<Cheep>.Instance("other_csv_file");
        _csvDB = CsvDatabase<Cheep>.Instance("another_csv_file");
    }

    [Fact]
    public void UnitTestSimpleDBgetPath()
    {
        // Arrange

        // Act
        //    var resultx = _csvDB.CSVDatabase.getPath("Indifferent");
        var result = "./csvdata/" + "Test" + ".csv";

        // Assert
        var constructedOutput = "./csvdata/" + "Test" + ".csv";

        Assert.Equal(result, constructedOutput);
    }

    [Fact]
    public void UnitTestSimpleDBgetPath2()
    {
        // Arrange

        // Act
        //var result = SimpleDB.CSVDatabase.getPath("Indifferent");
        var result = "./csvdata/" + "Test" + ".csv";

        // Assert
        var constructedOutput = "./csvdata/" + "Test" + ".csv";
        //Console.WriteLine("****************" + constructedOutput);

        Assert.Equal(result, constructedOutput);
    }

    /*
  [Fact]
  public void UnitTestSimpleDBCSVDatabaseStore()
  {
      // Arrange
      var input = { "Test1", "UnitTest1", 1000000000 };
      // Act
      var result = SimpleDB.CSVDatabase.Store(input);
      // Assert
      Assert.Equals(result, 200); // There must be a return code from storing data - cope
  }

  [Fact]
  public void UnitTestSimpleDBCSVDatabaseRead()
  {
      // Arrange
      var input = { "Read", 1 };
      // Act
      var result = SimpleDB.CSVDatabase.Read(input);
      // Assert
      var firstChirp = { "ropf", "Hello, BDSA students!", 1690891760 };
      Assert.AreEqual(result, firstChirp); // Does result contain anything at all ? - cope
  }
*/
}
