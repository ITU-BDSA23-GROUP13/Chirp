namespace Web.Tests;

public class UnitTestWeb

{
    /************************
        Dummy test
    *************************/
    [Fact]
    public void NinetynineIsNotPrime() // Dummy test
    {
        // Arrange
        //var input = 99;
        // Act
        var result = false;
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
        //var input = 99;
        // Act
        var result = false;
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void NotOKTestDBFacadeRunDB() // Dummy test
    {
        // Failure running DB

        // Arrange
        //var input = 99;
        // Act
        var result = false;
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
        //var input = 99;
        // Act
        var result = false;
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void NotOKTestDBCheepServiceGetCheeps() // Dummy test
    {
        // Cheeps are not extracted
        // The wrong Cheeps are extracted

        // Arrange
        //var input = 99;
        // Act
        var result = false;
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DummyTestFluentAssertion() // Dummy test

    //  https://medium.com/p/87c2e087c6d#62e8

    //  Testing with fluent Assertion

    // To be deleted after constidering whether this information is useful
    {
        string actual = "ABCDEFGHI";

        // Without Fluent Assertion
        Assert.True(actual.StartsWith("AB") && actual.EndsWith("HI"), "string does not start with AB and ends with HI");

        // With Fluent Assertion
        //actual.Should().StartWith("AB").And.EndWith("HI");

    }
}

