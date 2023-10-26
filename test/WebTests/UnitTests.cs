//hello

public class UnitTests

// Web

{
    [Fact]
    public void NinetynineIsNotPrime()
    {
        // Arrange
        var input = 99;
        // Act
        var result = _primeService.IsPrime(input);
        // Assert
        Assert.False(result);
    }

}
