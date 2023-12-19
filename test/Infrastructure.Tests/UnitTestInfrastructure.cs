/*********************************************
    UnitTest Chirp Repository
**********************************************/

using Chirp.Core;
using Chirp.Infrastructure;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Tests;

public class UnitTestsInfrastructure : IDisposable
{
    // https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Testing/TestingWithoutTheDatabase/SqliteInMemoryBloggingControllerTest.cs
    private readonly ChirpContext context;
    private readonly IAuthorRepository authorRepository;
    private readonly ICheepRepository cheepRepository;

    public UnitTestsInfrastructure()
    {
        // I think creating DbContexts by directly giving the connection strings
        // and letting it connect itself might be reusing the same connection.
        // So here we open new ones explicitly.
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(connection)
            .Options;
        
        context = new ChirpContext(options);
        context.Database.EnsureCreated();
        authorRepository = new AuthorRepository(context);
        cheepRepository = new CheepRepository(context);
    }

    public void Dispose()
    {
        Console.WriteLine("Disposed ChirpContext");
        context.Database.CloseConnection();
        context.Dispose();
    }

    private static readonly AuthorDTO testAuthor = new AuthorDTO {
        Name = "Test Author",
        Email = "test@email.com",
    };
    private static readonly CheepDTO testCheep = new CheepDTO {
        Author = testAuthor.Name,
        Text = "Test Cheep",
        Timestamp = (ulong) DateTimeOffset.Now.ToUnixTimeSeconds(),
    };

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

    [Fact]
    public async Task UnitTestGetNonExistingFollowing()
    {
        var following = await authorRepository.GetFollowing("ThisUserDoesNotExist", "ThisUserDoesNotExist");

        Assert.Null(following);
    }

    [Fact]
    public async Task UnitTestGetNotFollowing()
    {
        DBInitializer.SeedDatabase(context);

        var following = await authorRepository.GetFollowing("Jacqualine Gilcoine", "Mellie Yost");

        Assert.False(following);
    }

    [Fact]
    public async Task UnitTestPutFollowing()
    {
        DBInitializer.SeedDatabase(context);
        Assert.True(await authorRepository.PutFollowing("Jacqualine Gilcoine", "Mellie Yost"));

        var following = await authorRepository.GetFollowing("Jacqualine Gilcoine", "Mellie Yost");
        
        Assert.True(following);
    }

    [Fact]
    public async void UnitTestGetFollowerCount()
    {
        DBInitializer.SeedDatabase(context);
        Assert.True(await authorRepository.PutFollowing("Jacqualine Gilcoine", "Mellie Yost"));

        var a = await authorRepository.GetFollowerCount("Mellie Yost");
        Assert.NotNull(a);
        Assert.Equal((uint) 1, (uint) a);
        var b = await authorRepository.GetFollowerCount("Jacqualine Gilcoine");
        Assert.NotNull(b);
        Assert.Equal((uint) 0, (uint) b);
    }

}