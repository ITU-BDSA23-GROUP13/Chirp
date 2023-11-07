using Chirp.Infrastructure;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/*********************************************
    UnitTest Chirp Repository
**********************************************
Tests units:
    GetAuthorIdFromName(String author)
    ReadCheeps()
    ChirpRepository()
    ReadCheepsFromAuthor(ulong authorId)

Not implemented yet:
    CreateCheep(Guid id, CheepDTO cheepDTO)
    CreateAuthor(Guid id, AuthorDTO authorDTO)
    ReadAuthors()
    ReadCheep(Guid id)
    ReadAuthor(ulong id)
    UpdateAuthor(AuthorDTO author)
    UpdateCheep(CheepDTO cheep)
    DeleteCheep(Guid id)
    DeleteAuthor(Guid id)    

***********************************************/
namespace Chirp.Tests;

public class UnitTestsInfrastructure

{
    /*
    [Fact]
    public void UnitTestReadNumberOfCheeps()
    // Remove if too much disturbance
    {
        // Arrange
        var x = new ChirpRepository();
        var CountCheeps = 99;
        // Act
        var result = ReadNumberOfCheeps();
        // Assert
        Assert.False(result == CountCheeps);

    }

    [Fact]
    public int UnitTestReadNumberOfPagesOfCheeps()
    // Remove if too much disturbance
    {
        // Arrange
        var x = new ChirpRepository();
        var CountPages = 9;
        // Act
        var result = ReadNumberOfPagesOfCheeps();
        // Assert
        Assert.False(result == CountPages);
    }
    */

    [Fact]
    public async Task UnitTestGetAuthorIdFromName()
    {
        // Arrange
        var input = "Mellie Yost";
        //var input = "ropf";
        //var input = "Helge";
        var cR = new ChirpRepository();

        // Act
        // GetAuthorIdFromName returns a <ulong>-value
        var _ = await cR.GetAuthorIdFromName(input);
    }

    [Fact]
    public async Task NotUnitTEstGetAuthorIdFromName() // Dummy test
    {
        // Arrange
        var input = "XXXX";
        var cR = new ChirpRepository();
        // Act
        // var msg = await cR.GetAuthorIdFromName(input);
        Func<Task> action = async () => await cR.GetAuthorIdFromName(input);

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(action);
        Assert.Contains("Sequence contains no elements.", ex.Message);
    }
    /*
            [Fact]
            public async Task UnitTestReadCheeps() // Dummy test
            {
                // Arrange
                var input = 99;
                // Act
                var result = ReadCheeps();
                // Assert
                Assert.False(null(result));
            }

            [Fact]
            public async Task UnitTestChirpRepository() // Dummy test
            {
                // Arrange

                // Act
                var result = ChirpRepository();
                // Assert
                Assert.False(result);
            }

            [Fact]
            public async Task UnitTestReadCheepsFromAuthor() // Dummy test
            {
                // Arrange
                var input = 0;
                // Act
                var result = ReadCheepsFromAuthor(input);
                // Assert
                Assert.False(result);
            }
        */
}
