using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Web;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Web.Tests;

// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public class End2EndTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public End2EndTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Jacqualine%20Gilcoine")]
    [InlineData("/NonExistingUser")]
    public async void E2ETestContainsNavigationBarNotLoggedIn(string url)
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Public timeline", html);
        Assert.Contains("Register", html);
        Assert.Contains("Login", html);
    }

    [Theory]
    [InlineData("/")]
    public async void E2ETestContains32Cheeps(string url)
    {
        // Example of Cheep:
        //
        // <strong>
        //     <a href="/author">author</a>
        // </strong>
        // Text
        // <small>&mdash; today at 12.00</small>

        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        Console.WriteLine(html);

        Regex rx = new Regex(@"<strong>.*?<\/strong>", RegexOptions.Singleline);
        MatchCollection matches = rx.Matches(html);
        Assert.Equal(32, matches.Count);
    }

}