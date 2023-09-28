using static Program;
using Xunit;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualBasic;

namespace Chirp.CLI.Client.Tests
{
    public class UnitTest1
    {
        public record Cheep(string Author, string Message, long Timestamp);

        [Fact]
        public void ToBeDeleted_NoTest()
        {
            // Arrange

            // Act
            var test = true;
            // Assert
            Assert.True(test);

        }
        //[Fact]
        /*
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

                // Act NOT WORKING ATM
                //UserInterface.PrintCheeps(cheeps);

                // Assert
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }
        */
    }
}
