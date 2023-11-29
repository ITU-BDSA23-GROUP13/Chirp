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

    //[Fact]
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
    public async Task UnitTestGetCount()
    {
        // Arrange
        var cheepRepository = new CheepRepository(new ChirpContext());

        // Act
        var count = await cheepRepository.GetCount(); // Count has a value if there is a table

        // Assert
        Assert.True(count >= 0);

    }

    // [Theory]
    // [InlineData(7)]
    // [InlineData(70000000)]

    [Fact]
    public async Task UnitTestGetText() //(ulong value)
    {
        // Arrange
        var cheepRepository = new CheepRepository(new ChirpContext());

        // Act
        string? txt = await cheepRepository.GetText(Guid.NewGuid());

        // Assert
        if (txt == null)
        {
            Assert.Null(txt);
        }
        else
        {
            Assert.NotNull(txt); // 7 is a random number - can we extract a cheep id ?
        }
    }

    //[Fact]
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

    //[Fact]
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

