/*********************************************
    UnitTest Chirp Repository
**********************************************/

using Chirp.Core;
using Chirp.Infrastructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chirp.Tests;

public class UnitTestsInfrastructure
{
    private readonly ChirpContext context;
    private readonly IAuthorRepository authorRepository;
    private readonly ICheepRepository cheepRepository;

    private static readonly AuthorDTO testAuthor = new AuthorDTO {
        Name = "Test Author",
        Email = "test@email.com",
    };
    private static readonly CheepDTO testCheep = new CheepDTO {
        Author = testAuthor.Name,
        Text = "Test Cheep",
        Timestamp = (ulong) DateTimeOffset.Now.ToUnixTimeSeconds(),
    };

    private static uint counter = 0;

    public UnitTestsInfrastructure()
    {
        // In case tests are started on different threads, we stille want to ensure that each test use a different database.
        // https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked.increment?view=net-8.0
        uint i = Interlocked.Increment(ref counter);

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<CheepRepository>();
        builder.Services.AddSingleton<AuthorRepository>();
        builder.Services.AddDbContext<ChirpContext>(
            builder => builder.UseInMemoryDatabase("ChirpDB" + i)
        );
        var app = builder.Build();

        context = app.Services.GetRequiredService<ChirpContext>();
        authorRepository = app.Services.GetRequiredService<AuthorRepository>();
        cheepRepository = app.Services.GetRequiredService<CheepRepository>();
    }

    [Fact]
    public async Task UnitTestGetExistingUser()
    {
        // Arrenge
        await authorRepository.Put(testAuthor);

        // Act
        var author = await authorRepository.Get(testAuthor.Name);

        // Assert
        Assert.NotNull(author);
        Assert.Equal(testAuthor, author);
    }

    [Fact]
    public async Task UnitTestGetNonexistingAuthor()
    {
        // Act
        var author = await authorRepository.Get("UserThatDoesNotExist!");

        // Assert
        Assert.Null(author);
    }

    [Fact]
    public async Task UnitTestGetAuthorEmail()
    {
        // Arrange
        await authorRepository.Put(testAuthor);

        // Act
        var author = await authorRepository.GetEmail(testAuthor.Name);

        // Assert
        Assert.NotNull(author);
        Assert.Equal(testAuthor.Email, author);
    }

    [Fact]
    public async Task UnitTestGetEmailNonexistingUser()
    {
        // Act
        var email = await authorRepository.GetEmail("UserThatDoesNotExist!");

        // Assert
        Assert.Null(email);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    public async Task UnitTestGetAllCheepCount(int inputCount)
    {
        // Arrange
        await authorRepository.Put(testAuthor);

        for (var i = 0; i < inputCount; i++)
        {
            Assert.True(
                await cheepRepository.Put(new CheepDTO
                {
                    Author = testCheep.Author,
                    Text = testCheep.Text,
                    Timestamp = (ulong) DateTimeOffset.Now.ToUnixTimeSeconds(),
                })
            );
        }

        // Act
        var count = await cheepRepository.GetAllCount();

        // Assert
        Assert.Equal((uint) inputCount, count);
    }

    [Fact]
    public async Task UnitTestGetCheepsPageFromAuthorSortedByNewest()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var authors = await authorRepository.GetCheepsPage("Jacqualine Gilcoine", 0, 32, ICheepRepository.Order.Newest);

        // Assert
        Assert.NotNull(authors);
        Assert.Equal(32, authors.Count());
    }

    [Fact]
    public async Task UnitTestGetCheepsPageFromAuthorSortedByOldest()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var authors = await authorRepository.GetCheepsPage("Jacqualine Gilcoine", 0, 32, ICheepRepository.Order.Oldest);

        // Assert
        Assert.NotNull(authors);
        Assert.Equal(32, authors.Count());
    }

    [Fact]
    public async Task UnitTestGetCheepsFromNonExistingAuthor()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var authors = await authorRepository.GetCheepsPage("ThisUserDoesNotExist", 0, 1, 0);

        // Assert
        Assert.Null(authors);
    }

}