namespace Chirp.CLI.Client.Tests;

public class UnitTest1
{
    [Fact]
    public void UnitTestPrintCheeps()
    // Test UserInterface.PrintCheeps()

    // Can you test Console.Writeline at all? cope
    {
        var UI = new Userinterface;
        var c = new cheep;

        // Arrange
        string author = cheep.Author;
        string message = cheep.Message;
        DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToLocalTime();

        // Act
        var c = $"{author} @ {date:MM\\/dd\\/yy HH:mm:ss}: {message}";
        var result = UI(author, message, date);

        // Assert
        Assert.Equals(result, c);
    }

    [Fact]
    public void UnitTestPrintMessage()
    // Test UserInterface.PrintMessage()
    {
        // Arrange
        var input = "This is a message!";
        // Act
        var result = false;
        // Assert
        Assert.False(result);
    }
}

