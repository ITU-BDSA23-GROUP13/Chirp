namespace Chirp.SimpleDB.Tests;

public class UnitTest1
{
    [Fact]
    public void UnitTestSimpleDBCVSDatabaseStore()
    {
        // Arrange
        var input = { "Test1", "UnitTest1", 1000000000 };
        // Act
        var result = SimpleDB.CSVDatabase.Store(input);
        // Assert
        Assert.Equals(result, 200); // There must be a return code from storing data - cope
    }

    [Fact]
    public void UnitTestSimpleDBCVSDatabaseRead()
    {
        // Arrange
        var input = { "Read", 1 };
        // Act
        var result = SimpleDB.CSVDatabase.Read(input);
        // Assert
        var firstChirp = { "ropf", "Hello, BDSA students!", 1690891760 };
        Assert.AreEqual(result, firstChirp); // Does result contain anything at all ? - cope
    }
}
