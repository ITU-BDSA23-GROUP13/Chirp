using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Tests;

public class UnitTestWeb
{
    private readonly ICheepService service;

    public UnitTestWeb()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<ICheepService, CheepService>();
        builder.Services.AddSingleton<ICheepRepository, CheepRepository>();
        builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
        builder.Services.AddDbContext<ChirpContext>(
            builder => builder.UseInMemoryDatabase("ChirpDB")
        );
        var app = builder.Build();

        var context = app.Services.GetRequiredService<ChirpContext>();
        DBInitializer.SeedDatabase(context);

        service = app.Services.GetRequiredService<ICheepService>();
    }

    [Fact]
    public async void UnitTestGetCheepsAndPageCount()
    {
        var (cheeps, pageCount) = await service.GetCheepsAndPageCount(0);

        Assert.NotEmpty(cheeps);
        Assert.True(pageCount > 0);
    }

    [Fact]
    public async void UnitTestGetCheepsAndTotalCountFromAuthor()
    {
        var (cheeps, pageCount) = await service.GetCheepsAndTotalCountFromAuthor("Jacqualine Gilcoine", 0) ?? throw new NullReferenceException();

        Assert.NotEmpty(cheeps);
        Assert.True(pageCount > 0);
    }

    //[Fact]
    public void UnitTestGetCheepsAndTotalCountFromFollowed()
    {
    }

    //[Fact]
    public void UnitTestPutCheep()
    {
    }

    //[Fact]
    public void UnitTestFollowAuthor()
    {
    }

    //[Fact]
    public void UnitTestUnfollowAuthor()
    {
    }
}