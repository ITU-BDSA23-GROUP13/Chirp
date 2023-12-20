using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Web.Tests;

public class UnitTestWeb : IDisposable
{
    private readonly ChirpContext context;
    private readonly ICheepService service;

    public UnitTestWeb()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<ICheepService, CheepService>();
        builder.Services.AddSingleton<ICheepRepository, CheepRepository>();
        builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
        builder.Services.AddDbContext<ChirpContext>(options =>
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            options.UseSqlite(connection);
        });
        builder.Logging.ClearProviders();
        var app = builder.Build();

        context = app.Services.GetRequiredService<ChirpContext>();
        context.Database.EnsureCreated();
        DBInitializer.SeedDatabase(context);

        service = app.Services.GetRequiredService<ICheepService>();
    }

    public void Dispose()
    {
        Console.WriteLine("Disposed ChirpContext");
        context.Database.CloseConnection();
        context.Dispose();
    }

    [Fact]
    public async void UnitTestGetCheepsAndPageCount()
    {
        var (cheeps, pageCount) = await service.GetCheepsAndPageCount(1);

        Assert.NotEmpty(cheeps);
        Assert.True(pageCount > 0);
    }

    [Fact]
    public async void UnitTestGetCheepsAndTotalCountFromAuthor()
    {
        var (cheeps, pageCount) = await service.GetCheepsAndTotalCountFromAuthor("Jacqualine Gilcoine", 1) ?? throw new NullReferenceException();

        Assert.NotEmpty(cheeps);
        Assert.True(pageCount > 0);
    }

    [Fact]
    public async void UnitTestGetCheepsAndTotalCountFromFollowed()
    {
        Assert.True(await service.PutFollowing("Jacqualine Gilcoine", "Mellie Yost"));

        var (mellieCheeps, melliePageCount) = await service.GetCheepsAndTotalCountFromAuthor("Mellie Yost", 1) ?? throw new NullReferenceException();
        var (cheeps, pageCount) = await service.GetCheepsAndTotalCountFromFollowed("Jacqualine Gilcoine", 1) ?? throw new NullReferenceException();

        Assert.Equal(mellieCheeps, cheeps);
        Assert.Equal(melliePageCount, pageCount);
    }

    [Fact]
    public async void UnitTestFollowAuthor()
    {
        Assert.True(await service.PutFollowing("Jacqualine Gilcoine", "Mellie Yost"));

        var followed = await service.GetFollowsAll("Jacqualine Gilcoine");
        Assert.NotNull(followed);
        Assert.Contains("Mellie Yost", followed);
    }

    [Fact]
    public async void UnitTestUnfollowAuthor()
    {
        Assert.True(await service.PutFollowing("Jacqualine Gilcoine", "Mellie Yost"));
        Assert.True(await service.DeleteFollowing("Jacqualine Gilcoine", "Mellie Yost"));
        Assert.False(await service.GetFollow("Jacqualine Gilcoine", "Mellie Yost"));
    }

    [Fact]
    public async void UnitTestGetFollowerCount()
    {
        Assert.True(await service.PutFollowing("Jacqualine Gilcoine", "Mellie Yost"));

        var a = await service.GetFollowerCount("Mellie Yost");
        Assert.NotNull(a);
        Assert.Equal((uint) 1, (uint) a);
        var b = await service.GetFollowerCount("Jacqualine Gilcoine");
        Assert.NotNull(b);
        Assert.Equal((uint) 0, (uint) b);
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

        var cheeps = (await service.GetCheepsAndPageCount(1)).Item1;
        Assert.Contains(cheep, cheeps);
    }

}