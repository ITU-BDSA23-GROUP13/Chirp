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
            // var expectedOutput = "User1 @ 09/26/21 00:00:00 Output er for kort!\r\n";
            var expectedOutput = "Output er for kort!" + Environment.NewLine;

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                UserInterface.PrintMessage("Output er for kort!");
                wasExecuted = true;

                var x = sw.ToString();

                // Assert
                Assert.True(wasExecuted);
                Assert.Equal(expectedOutput, sw.ToString());
            }
        }

    }

}
