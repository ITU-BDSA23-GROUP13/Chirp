using static Program;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualBasic;


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


    [Fact]
    public void UnitTestPrintMessage()
    // Test UserInterface.PrintMessage()
    {
        // Arrange
        var input = "This is a message!";
        // Act
        //------------- var result = UserInterface.PrintMessage(input); // Cannot assign void to an implicitly-typed variable (CS0815)
        // Assert
        //------------- Assert.Equal(result, "");
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

        //Act
        var c = new Cheep(author, message, ts);
        //var result = UserInterface.PrintMessage(c);  // Cannot assign void to an implicitly-typed variable (CS0815)
        //var result = UserInterface.PrintMessage(author, message, date);

        var result = new Cheep(author, message, ts); // To be replaced with something meaningfull

        // Assert
        Assert.Equal(result, c);
    }


using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Chirp.CLI.Client.Tests
{
    public class UnitTest1
    {
        public record Cheep(string Author, string Message, long Timestamp);

                [Fact]
        public void PrintCheeps_PrintsCheepsCorrectly()
        {
            // Arrange
            var cheeps = new List<Cheep>
            {
                new Cheep("User1", "Hello", 1632632400)
        };
            var expectedOutput = "User1 @ 09/26/21 00:00:00: Hello\r\n";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                UserInterface.PrintCheeps(cheeps);

                // Assert
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }
    }
}
