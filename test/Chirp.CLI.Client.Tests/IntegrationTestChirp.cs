using static Program;
using Xunit;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualBasic;

namespace Chirp.CLI.Client.Tests
{
    public class IntegrationTestChirp
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
    }
}
