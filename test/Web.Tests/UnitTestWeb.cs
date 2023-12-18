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

    [Fact]
    public async void UnitTestGetCheepsAndTotalCountFromFollowed()
    {
        var (cheeps, pageCount) = await service.GetCheepsAndTotalCountFromFollowed("Jacqualine Gilcoine", 0) ?? throw new NullReferenceException();

        Assert.NotEmpty(cheeps);
        Assert.True(pageCount > 0);
    }

    [Fact]
    public async void UnitTestFollowAuthor()
    {
        Assert.True(await service.PutFollower("Jacqualine Gilcoine", "Mellie Yost"));

        var (mellieCheeps, melliePageCount) = await service.GetCheepsAndTotalCountFromAuthor("Mellie Yost", 0) ?? throw new NullReferenceException();
        var (cheeps, pageCount) = await service.GetCheepsAndTotalCountFromFollowed("Jacqualine Gilcoine", 0) ?? throw new NullReferenceException();

        Assert.Equal(mellieCheeps, cheeps);
        Assert.Equal(melliePageCount, pageCount);
    }

    [Fact]
    public async void UnitTestUnfollowAuthor()
    {
        Assert.True(await service.PutFollower("Jacqualine Gilcoine", "Mellie Yost"));
        Assert.True(await service.DeleteFollow("Jacqualine Gilcoine", "Mellie Yost"));
    }

    [Fact]
    public async void UnitTestPutCheep()
    {
        var cheep = new CheepViewModel
        {
            Author = "Jacqualine Gilcoine",
            Message = "Test Cheep",
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.Now.ToUnixTimeSeconds()),
        };

        await service.PutCheep(cheep);

        var cheeps = (await service.GetCheepsAndPageCount(0)).Item1;
        Assert.Contains(cheep, cheeps);
    }

}