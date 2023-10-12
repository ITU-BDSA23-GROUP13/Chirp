namespace Chirp.CLI.Client.Tests
{
    public class IntegrationTestChirpUserinterface
    {
        public record Cheep(string Author, string Message, long Timestamp);

        [Fact]
        public void UserInterfacePrintMessageTest2()
        {
            // Arrange
            var expectedOutput = "Output er for kort!" + Environment.NewLine;

            // Act
            using var sw = new StringWriter();
            Console.SetOut(sw);
            UserInterface.PrintMessage("Output er for kort!");

            // Assert
            Assert.Equal(expectedOutput, sw.ToString());
        }

        [Theory]
        [InlineData("User1", "Output er for kort!", 1632600000, "User1 @ 09/25/21 20:00:00: Output er for kort!")]
        [InlineData("User2", "Output er for kort!", 1632600000, "User2 @ 09/25/21 20:00:00: Output er for kort!")]
        [InlineData("User3", "Output er for kort!", 1632600000, "User3 @ 09/25/21 20:00:00: Output er for kort!")]
        public void UserInterfacePrintCheepsTest1(string Author, string message, long TimeStamp, string expectedOutput)
        {
            // Arrange
            bool wasExecuted = false;
            var cheeps = new List<Program.Cheep>();
            cheeps.Add(new Program.Cheep(Author, message, TimeStamp));

            // Act
            using var sw = new StringWriter();
            Console.SetOut(sw);
            UserInterface.PrintCheeps(cheeps);
            wasExecuted = true;

            // Assert
            Assert.True(wasExecuted);
            Assert.Equal(expectedOutput + Environment.NewLine, sw.ToString());
        }
    }
}