/*********************************************
    UnitTest Chirp Repository
**********************************************/

using Chirp.Infrastructure;

namespace Chirp.Tests;

using static Chirp.Core.ICheepRepository;

public class UnitTestsInfrastructure
{

    [Fact]
    public void IntegrationTestDummyInfrastructure() // Dummy test
    {
        // Arrange
        //var input = 99;
        // Act
        var result = false;
        // Assert
        Assert.False(result);
    }


    // [Fact]
    // public async Task NotUnitTestGetAuthor() // Dummy test
    // {
    //     // Arrange
    //     var input = 1000000;
    //     var cR = new CheepRepository.ChirpContext();

    //     // Act
    //     Func<Task> action = async () => await cR.GetAuthor(input);

    //     // Assert
    //     var ex = await Assert.ThrowsAsync<InvalidOperationException>(action);
    //     Assert.Contains("Sequence contains no elements.", ex.Message);
    // }

}

