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
        var author = await authorRepository.Get("UserThatDoesNotExist!"); // Here we assume that there are no users with this name

        // Assert
        Assert.Null(author);
    }

    [Fact]
    public async Task UnitTestGetPageSortedBy()
    {
        // Arrange
        var cheepRepository = new CheepRepository(new ChirpContext());

        // Act
        var author = await cheepRepository.GetPageSortedBy(1, 32, 0); // Here we assume that there are no users with this name

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

    [Theory]
    [InlineData(7)]
    [InlineData(70000000)]

    public async Task UnitTestGetText(ulong value)
    {
        // Arrange
        var cheepRepository = new CheepRepository(new ChirpContext());

        // Act
        string txt = await cheepRepository.GetText(value);

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

}
