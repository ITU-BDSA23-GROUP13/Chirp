using static Program;

namespace Chirp.CLI.Client.Tests;

public class UnitTest1
{
    public record Cheep(string Author, string Message, long Timestamp);

    [Fact]
    public void UnitTestTestMethodINUserInterface()
    // To be deleted when methods start returning values to test

    // Test UserInterface.TestRunUnitTest1()
    {
        // Arrange
        var input = "This is a message!";
        // Act
        var result = UserInterface.TestRunUnitTest1(input);
        // Assert
        Assert.Equal(result, "******" + input);

    }


    [Theory]
    [InlineData(1000000000)]
    [InlineData(2000000000)]
    [InlineData(3000000000)]

    public void UnitTestPrintCheeps(long TS)
    // Test UserInterface.PrintCheeps(TS)
    {
        // Arrange
        string author = "TEST";
        string message = "******";
        long ts = TS;

        // 
        var c = new Cheep(author, message, ts);
        //var result = UserInterface.PrintMessage(c);  // Cannot assign void to an implicitly-typed variable (CS0815)
        //var result = UserInterface.PrintMessage(author, message, date);

        var result = new Cheep(author, message, ts); // To be replaced with something meaningfull

        // Assert
        Assert.Equal(result, c);
    }

}
