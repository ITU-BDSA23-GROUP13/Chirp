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

    /************************
        Test DBFacade.cs
    *************************/
    [Fact]
    public void OKTestDBFacadeRunDB() // Dummy test
    {
        // DB is running

        // Arrange
        var input = 99;
        // Act
        var result = _primeService.IsPrime(input);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void NotOKTestDBFacadeRunDB() // Dummy test
    {
        // Failure running DB

        // Arrange
        var input = 99;
        // Act
        var result = _primeService.IsPrime(input);
        // Assert
        Assert.False(result);
    }


    /************************
        Test CheeoService.cs
    *************************/
    [Fact]
    public void OKTestDBCheepServiceGetCheeps() // Dummy test
    {
        // Cheeps are extracted

        // Arrange
        var input = 99;
        // Act
        var result = _primeService.IsPrime(input);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void NotOKTestDBCheepServiceGetCheeps() // Dummy test
    {
        // Cheeps are not extracted
        // The wrong Cheeps are extracted

        // Arrange
        var input = 99;
        // Act
        var result = _primeService.IsPrime(input);
        // Assert
        Assert.False(result);
    }

}
