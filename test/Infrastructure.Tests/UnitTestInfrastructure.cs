// namespace Infrastructure.Tests; 
// this needs to be moved !!!!

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
using Chirp.Infrastructure;
//namespace Chirp;
using Chirp;

public class UnitTestsInfrastructure

{
    [Fact]
    public async Task UnitTEstGetAuthorIdFromName()
    {
        // Arrange
        var input = "Mellie Yost";
        //var input = "ropf";
        var x = new ChirpRepository();

        // Act
        console.Writeline("**************");
        // GetAuthorIdFromName returns a <ulong>-value
        var author = await x.GetAuthorIdFromName(input);
        console.Writeline(author.ToString());

        var result = false;
        if (author > 0)
        {
            result = true;
        }

        // Assert
        Assert.false(result == true);
    }

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
