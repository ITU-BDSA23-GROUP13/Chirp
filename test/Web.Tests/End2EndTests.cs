using Chirp.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Data.Common;
using System.Text.RegularExpressions;

using static Web.Tests.End2EndTests;

namespace Web.Tests;

// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public class End2EndTests : IClassFixture<CustomWebApplicationFactory>
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // These variables are not needed, but the web app throws if they are null.
            Environment.SetEnvironmentVariable("CHIRP_GITHUB_CLIENT_SECRET", "x");

            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ChirpContext>));

                services.Remove(dbContextDescriptor!);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor!);

                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<ChirpContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });
            });
        }
    }

    private readonly WebApplicationFactory<Program> factory;

    public End2EndTests(CustomWebApplicationFactory factory)
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

        Regex rx = new Regex(@"<strong>.*?<\/strong>", RegexOptions.Singleline);
        MatchCollection matches = rx.Matches(html);
        Assert.Equal(32, matches.Count);
    }

}