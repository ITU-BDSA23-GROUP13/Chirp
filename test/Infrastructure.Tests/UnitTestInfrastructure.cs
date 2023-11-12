/*********************************************
    UnitTest Chirp Repository
**********************************************/

using Chirp.Infrastructure;

namespace Chirp.Tests;

public class UnitTestsInfrastructure
{

    [Fact]
    public async Task UnitTestGetExistingUser()
    {
        // Arrange
        var authorRepository = new AuthorRepository(new ChirpContext());

        // Act
        var author = await authorRepository.Get("Mellie Yost"); // Here we assume that there is a user with this name

        // Assert
        Assert.NotNull(author);
    }

    [Fact]
    public async Task UnitTestGetNonexistingUser()
    {
        // Arrange
        var authorRepository = new AuthorRepository(new ChirpContext());

        // Act
        Func<Task> action = async () => await authorRepository.Get("UserThatDoesNotExist!"); // Here we assume that there are no users with this name

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(action);
        Assert.Contains("Sequence contains no elements.", ex.Message);
        //Assert.Null(author);
    }

    [Fact]
    public async Task UnitTestGetEmail()
    {
        // Arrange
        var authorRepository = new AuthorRepository(new ChirpContext());

        // Act
        var author = await authorRepository.GetEmail("Mellie Yost"); // Here we assume that there is a user with this name

        // Assert
        Assert.NotNull(author);
    }

    [Fact]
    public async Task UnitTestGetEmailNonexistingUser()
    {
        // Arrange
        var authorRepository = new AuthorRepository(new ChirpContext());

        // Act
        Func<Task> action = async () => await authorRepository.GetEmail("UserThatDoesNotExist!"); // Here we assume that there are no users with this name

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(action);
        Assert.Contains("Sequence contains no elements.", ex.Message);
        //Assert.Null(author);
    }

    [Fact]
    public async Task UnitTestGetCheepsPageSortedByAscending()
    {
        // Arrange
        var authorRepository = new AuthorRepository(new ChirpContext());

        // Act
        //GetCheepsPageSortedBy(string name, uint page, uint pageSize, Order order)
        var authors = await authorRepository.GetCheepsPageSortedBy("Mellie Yost", 1, 32, 0);

        // Assert
        Assert.NotNull(authors);
    }

    [Fact]
    public async Task UnitTestGetCheepsPageSortedByDescending()
    {
        // Arrange
        var authorRepository = new AuthorRepository(new ChirpContext());

        // Act
        //GetCheepsPageSortedBy(string name, uint page, uint pageSize, Order order)
        var authors = await authorRepository.GetCheepsPageSortedBy("Mellie Yost", 1, 32, 0);

        // Assert
        Assert.NotNull(authors);
    }

    [Fact]
    public async Task UnitTestGetCheepsPageSortedByNonexistingUser()
    {
        // Arrange
        var authorRepository = new AuthorRepository(new ChirpContext());

        // Act
        //Func<Task> action = async () => await authorRepository.GetCheepsPageSortedBy("ThisUserDoesNotExist", 1, 32, 0);
        var authors = await authorRepository.GetCheepsPageSortedBy("ThisUserDoesNotExist", 1, 32, 0);

        // Assert
        //Assert.Null(authors);
        //Assert.NotEmpty(authors);

        Console.WriteLine("authors");
        Console.WriteLine(authors);

    }


}

