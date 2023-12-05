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

    public UnitTestsInfrastructure()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<CheepRepository>();
        builder.Services.AddSingleton<AuthorRepository>();
        builder.Services.AddDbContext<ChirpContext>(
            builder => builder.UseInMemoryDatabase("ChirpDB")
        );
        var app = builder.Build();

        context = app.Services.GetRequiredService<ChirpContext>();
        authorRepository = app.Services.GetRequiredService<AuthorRepository>();
        cheepRepository = app.Services.GetRequiredService<CheepRepository>();
    }

    [Fact]
    public async Task UnitTestGetExistingUser()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var author = await authorRepository.Get("Mellie Yost");

        // Assert
        Assert.NotNull(author);
    }

    //[Fact]
    public async Task UnitTestGetNonexistingUser()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var author = await authorRepository.Get("UserThatDoesNotExist!");

        // Assert
        Assert.Null(author);
    }

    [Fact]
    public async Task UnitTestGetEmail()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var author = await authorRepository.GetEmail("Mellie Yost");

        // Assert
        Assert.NotNull(author);
    }

    [Fact]
    public async Task UnitTestGetAllCount()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var count = await cheepRepository.GetAllCount();

        // Assert
        Assert.True(count >= 0);

    }

    // [Theory]
    // [InlineData(7)]
    // [InlineData(70000000)]

    public async Task UnitTestGetText() //(ulong value)
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var text = await cheepRepository.GetText(Guid.NewGuid());

        // Assert
        Assert.NotNull(text);
    }

    [Fact]
    public async Task UnitTestGetEmailNonexistingUser()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var email = await authorRepository.GetEmail("UserThatDoesNotExist!"); // Here we assume that there are no users with this name

        // Assert
        Assert.Null(email);
    }

    [Fact]
    public async Task UnitTestGetCheepsPageSortedByAscending()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var authors = await authorRepository.GetCheepsPageSortedBy("Mellie Yost", 1, 32, 0);

        // Assert
        Assert.NotNull(authors);
        Assert.Equal(authors.Count, 32);
    }

    [Fact]
    public async Task UnitTestGetCheepsPageSortedByDescending()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var authors = await authorRepository.GetCheepsPageSortedBy("Mellie Yost", 1, 32, 0);

        // Assert
        Assert.NotNull(authors);
        Assert.Equal(authors.Count, 32);
    }

    [Fact]
    public async Task UnitTestGetCheepsPageSortedByNonexistingUser()
    {
        // Arrange
        DBInitializer.SeedDatabase(context);

        // Act
        var authors = await authorRepository.GetCheepsPageSortedBy("ThisUserDoesNotExist", 1, 32, 0);

        // Assert
        Assert.Null(authors);
    }

}