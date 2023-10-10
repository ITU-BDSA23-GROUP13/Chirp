using static Chirp.CLI.Program;
using Xunit;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Chirp.CLI.Client.Tests
{
    public class IntegrationTestChirpUserinterface
    {
        public record Cheep(string Author, string Message, long Timestamp);

        [Fact]
        public void UserInterfacePrintMessageTest1()
        {
            // Arrange
            bool wasExecuted = false;

            // Act
            UserInterface.PrintMessage("******");
            wasExecuted = true;

            // Assert
            Assert.True(wasExecuted);

        }

        [Fact]
        public void UserInterfacePrintMessageTest2()
        {
            // Arrange
            bool wasExecuted = false;

            var expectedOutput = "Output er for kort!" + Environment.NewLine;

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                UserInterface.PrintMessage("Output er for kort!");
                wasExecuted = true;

                // Assert
                Assert.True(wasExecuted);
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }

        [Theory]
        [InlineData("User1", "Output er for kort!", 1632600000, "User1 @ 09/25/21 22:00:00: Output er for kort!")]
        [InlineData("User2", "Output er for kort!", 1632600000, "User2 @ 09/25/21 22:00:00: Output er for kort!")]
        [InlineData("User3", "Output er for kort!", 1632600000, "User3 @ 09/25/21 22:00:00: Output er for kort!")]
        public void UserInterfacePrintCheepsTest1(string Author, string message, long TimeStamp, string expectedOutput)
        {
            // Arrange
            bool wasExecuted = false;
            var cheeps = new List<Program.Cheep>();
            cheeps.Add(new Program.Cheep(Author, message, TimeStamp));

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                UserInterface.PrintCheeps(cheeps);
                wasExecuted = true;

                // Assert
                Assert.True(wasExecuted);
                Assert.Equal(expectedOutput + Environment.NewLine, sw.ToString());
            }
        }
    }
}