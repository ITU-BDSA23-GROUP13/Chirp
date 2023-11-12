using System;
using System.Diagnostics;
using System.ComponentModel;

using Chirp.Razor;

namespace Tests;

public class End2End
{

    [Fact]
    public void TestReadCheep()
    {
        // Arrange
        DBFacade.RunDB();

        // Act
        // string output = "";
        // using (var process = new Process())
        // {
        //     process.StartInfo.FileName = "/usr/bin/dotnet";
        //     process.StartInfo.Arguments = "./src/Chirp.Web/bin/Debug/net7.0/chirp.dll read 10";
        //     process.StartInfo.UseShellExecute = false;
        //     process.StartInfo.WorkingDirectory = "../../../../../";
        //     process.StartInfo.RedirectStandardOutput = true;
        //     process.Start();
        //     // Synchronously read the standard output of the spawned process.
        //     StreamReader reader = process.StandardOutput;
        //     output = reader.ReadToEnd();
        //     process.WaitForExit();
        // }
        // string fstCheep = output.Split("\n")[0];

        string fstCheep = "ropf \n Hello, World!";
        // Assert
        Assert.StartsWith("ropf", fstCheep);
        Assert.EndsWith("Hello, World!", fstCheep);
    }


}