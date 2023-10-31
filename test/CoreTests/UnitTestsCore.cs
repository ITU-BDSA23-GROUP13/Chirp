//hello 2

public class UnitTests

// Infrastructure

{
    [Fact]
    public void NinetynineIsNotPrime() // Dummy test
    {
        // Arrange
        var input = 99;
        // Act
        var result = _primeService.IsPrime(input);
        // Assert
        Assert.False(result);
    }

}
