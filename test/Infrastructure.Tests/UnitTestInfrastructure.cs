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
    [Fact]
    public void UnitTestDummyInfrastructure() // Dummy test
    {
        // Arrange
        //var input = 99;
        // Act
        var result = false;
        // Assert
        Assert.False(result);
    }
    /*
       [Fact]
       public async Task UnitTEstGetAuthorIdFromName()
       {
           // Arrange
           var input = "Mellie Yost";
           //var input = "ropf";
           //var input = "Helge";
           var x = new ChirpRepository();

           // Act
           console.Writeline("**************");
           // GetAuthorIdFromName returns a <ulong>-value
           var authorId = await x.GetAuthorIdFromName(input);
           console.Writeline(author.ToString());

           var result = false;
           if (authorId > 0)
           {
               result = true;
           }

           // Assert
           Assert.false(result);           // No author found
           Assert.true(authorId == 0);     // Value for No author found = 0 
       }
   */

    /*
        [Fact]
        public async Task NotUnitTEstGetAuthorIdFromName() // Dummy test
        {
            // Arrange
            var input = "XXXX";
            // Act
            var author = GetAuthorIdFromName(input);

            var result;
            if (author == null)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            // Assert
            Assert.False(result);
        }

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
