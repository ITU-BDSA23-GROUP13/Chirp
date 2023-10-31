namespace Infrastructure.Tests;

public class UnitTests

// Infrastructure

{
    /************************
        Dummy test
    *************************/
    [Fact]
    public void NinetynineIsNotPrime() // Dummy test
    {
        // Arrange
        var input = 99;
        // Act
        var result = false;
        // Assert
        Assert.False(result);
    }


}