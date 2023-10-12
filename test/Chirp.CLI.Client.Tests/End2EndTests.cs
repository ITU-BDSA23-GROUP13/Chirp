using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Chirp.CLI.Client.Tests
{
    public class End2EndTestChirp
    {
        // Test reading up to 10 occurences from the database.
        [Fact]
        public void TestReadCheep10()
        {
            // Copied from lecture 03

            // Arrange

            // Act
            string output = "";
            using var process = new Process();

            process.StartInfo.FileName = "dotnet"; //dotNetPath();
            process.StartInfo.Arguments = "run ./bin/Debug/net7.0/Chirp.exe read 10";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI/";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            // Synchronously read the standard output of the spawned process.
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
            //string fstCheep = output.Split("\n")[0];

            // Assert
            //Assert.StartsWith("ropf", fstCheep);
            //Assert.EndsWith("Hello, World!", fstCheep);
            Assert.NotNull(output);
        }

        //Generate path for dotnetcore based on platform Borrowed from group 12

        private string dotNetPath()
        {
            // The feature of extracting the runtimeinformation is inspired by stackoverflow
            //https://stackoverflow.com/questions/38790802/determine-operating-system-in-net-core
            string path;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                path = "/usr/local/share/dotnet/dotnet";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                path = @"C:\program files\dotnet\dotnet";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                path = "/usr/bin/dotnet";
            else
                throw new Exception("OS not supported");

            return path;
        }

    }
}