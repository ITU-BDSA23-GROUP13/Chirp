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
        var result = _primeService.IsPrime(input);
        // Assert
        Assert.False(result);
    }


}
