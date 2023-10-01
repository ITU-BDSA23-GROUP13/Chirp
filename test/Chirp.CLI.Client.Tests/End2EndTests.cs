using static Program;
using Xunit;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace Chirp.CLI.Client.Tests
{
    public class IntegrationTestChirp
    {
        //public record Cheep(string Author, string Message, long Timestamp);

        /*
        // Copy from lecture 03

        [Fact]
        public void TestReadCheep()
        {
            // Arrange
            ArrangeTestDatabase();

            // Act
            string output = "";
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/bin/dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI.Client/bin/Debug/net7.0/chirp.dll read 10";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                output = reader.ReadToEnd();
                process.WaitForExit();
            }
            string fstCheep = output.Split("\n")[0];
            // Assert
            Assert.StartsWith("ropf", fstCheep);
            Assert.EndsWith("Hello, World!", fstCheep);
        }
        */

        [Fact]
        public void ToBeDeleted_NotAnEnd2EndTest()
        {
            // Arrange
            bool wasExecuted = false;

            // Act
            UserInterface.PrintMessage("******");
            wasExecuted = true;

            // Assert
            Assert.True(wasExecuted);

        }

        /*
                private string start_Process_For_Client_CLI(string cmd)
                {
                    string output;
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "dotnet";
                        process.StartInfo.Arguments = "run " + cmd;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.CreateNoWindow = false;
                        process.StartInfo.WorkingDirectory = Path.Combine("..", "..", "..", "..", "..", "src", "Chirp.CLI");

                        process.Start();
                        StreamReader reader = process.StandardOutput;
                        output = reader.ReadToEnd();

                        process.WaitForExit();

                        return output;
                    }
                } */
    }
}

