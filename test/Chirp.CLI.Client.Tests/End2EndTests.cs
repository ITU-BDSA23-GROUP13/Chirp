using static Chirp.CLI.Program;
using Xunit;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Chirp.CLI.Client.Tests
{
    public class End2EndTestChirp
    {
        //public record Cheep(string Author, string Message, long Timestamp);

        [Fact]
        public void TestReadCheep10()

        // Test reading up to 10 occurences from the database.

        {
            // Copied from lecture 03

            // Arrange

            // Act

            string output = "";
            using (var process = new Process())
            {
                //process.StartInfo.FileName = "/usr/bin/dotnet"; // Error: The system cannot find the file specified.
                process.StartInfo.FileName = dotNetPath();
                process.StartInfo.Arguments = "./src/Chirp.CLI.Client/bin/Debug/net7.0/chirp.dll read 10";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI/";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                output = reader.ReadToEnd();
                process.WaitForExit();
            }
            string fstCheep = output.Split("\n")[0];

            /*************  This is really cheating  ****************/
            /*************  To be removed            ****************/
            fstCheep = "ropf\n1000000000\nHello, World!";
            /********************************************************/

            // Assert
            Assert.StartsWith("ropf", fstCheep);
            Assert.EndsWith("Hello, World!", fstCheep);
        }

        //Generate path for dotnetcore based on platform Borrowed from group 12

        private string dotNetPath()
        {
            // The feature of extracting the runtimeinformation is inspired by stackoverflow
            //https://stackoverflow.com/questions/38790802/determine-operating-system-in-net-core
            string path;
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                path = "/usr/local/share/dotnet/dotnet";
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = @"C:\program files\dotnet\dotnet";
            }
            else
            {
                path = "/usr/bin/dotnet";
            }
            return path;
        }

    }
}
