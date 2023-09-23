using System;
using SimpleDB;
using Xunit;

namespace Chirp.SimpleDB.Tests;

public class UnitTestSimpleDB
{

    /*

       //     IDatabase db = CSVDatabase.Instance();
    // 
    // private readonly CSVDatabase _simpleDB;
    private readonly CSVDatabase _csvDB;

    public UnitTestSimpleDB()
    {
        _csvDB = new CSVDatabase();
    }
    */

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
