using Chirp.Core;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

using static Chirp.Core.ICheepRepository;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext context;

    public CheepRepository(ChirpContext context)
    {
        this.context = context;
    }

    private static readonly IComparer<Cheep> newestComparer = Comparer<Cheep>.Create((a, b) => b.Timestamp.CompareTo(a.Timestamp));
    private static readonly IComparer<Cheep> oldestComparer = Comparer<Cheep>.Create((a, b) => a.Timestamp.CompareTo(b.Timestamp));

    private static IComparer<Cheep> getCheepComparer(Order order)
    {
        return order switch
        {
            Order.Newest => newestComparer,
            Order.Oldest => oldestComparer,
            _ => throw new System.Diagnostics.UnreachableException(),
        };
    }

    public async Task<IList<CheepDTO>> GetPageFromAll(uint page, uint pageSize, Order order)
    {
        var cheeps = order switch
        {
            Order.Newest => context.Cheep.OrderByDescending(c => c.Timestamp),
            Order.Oldest => context.Cheep.OrderBy(c => c.Timestamp),
            _ => throw new System.Diagnostics.UnreachableException(),
        };

        var list = await cheeps
            .Skip((int) ((page - 1) * pageSize))
            .Take((int) pageSize)
            .Select(c =>
                new CheepDTO
                {
                    Author = c.Author.UserName!,
                    Text = c.Text,
                    Timestamp = (ulong) c.Timestamp,
                }
            )
            .ToListAsync();

        if (list.Any(c => c.Author is null))
            throw new System.NullReferenceException();

        return list;
    }

    public async Task<uint> GetAllCount()
    {
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/checked-and-unchecked
        // We want this to throw an exception if the cast fails.
        return checked ((uint) await this.context.Cheep.CountAsync());
    }

    public async Task<IList<CheepDTO>?> GetPageFromFollowed(string followerName, uint page, uint pageSize, Order order = Order.Newest)
    {
        var cheeps = await context.Author
            .Include(a => a.Followers).ThenInclude(a => a.Cheeps).ThenInclude(c => c.Author)
            //.Where(a => a.UserName == followerName) // Doing it like this doesn't work for some reason.
            .SelectMany(a => a.Followers.SelectMany(a => a.Cheeps), (follower, cheep) => new {follower, cheep})
            .Where(f => f.follower.UserName == followerName && f.cheep != null)
            .Select(f => f.cheep)
            .ToListAsync();

        cheeps.Sort(getCheepComparer(order));

        var list = cheeps
            .Skip((int) ((page - 1) * pageSize))
            .Take((int) pageSize)
            .Select(c =>
                new CheepDTO
                {
                    Author = c.Author.UserName!,
                    Text = c.Text,
                    Timestamp = (ulong) c.Timestamp,
                }
            )
            .ToList();
            
        if (list.Any(c => c.Author is null))
            throw new System.NullReferenceException();

        return list;
    }

    public async Task<uint?> GetFollowedCount(string followerName)
    {
        var author = await context.Author
            .Include(a => a.Followers).ThenInclude(a => a.Cheeps)
            .Where(a => a.UserName == followerName)
            .FirstOrDefaultAsync();
        if (author is null) return null;

        var cheeps = author.Followers
            .SelectMany(a => a.Cheeps)
            .Count();

        return checked ((uint?) cheeps);
    }

    public async Task<CheepDTO?> Get(Guid id)
    {
        var cheep = await context.Cheep
            .Where(c => c.Id == id.ToString())
            .FirstOrDefaultAsync();
        if (cheep is null) return null;

        return new CheepDTO
        {
            Author = cheep.Author.UserName ?? throw new System.NullReferenceException(),
            Text = cheep.Text,
            Timestamp = (ulong) cheep.Timestamp,
        };
    }

    public async Task<AuthorDTO?> GetAuthor(Guid id)
    {
        var author = await context.Cheep
            .Where(c => c.Id == id.ToString())
            .Select(c => c.Author)
            .FirstOrDefaultAsync();
        if (author is null) return null;

        return new AuthorDTO
        {
            Name = author.UserName ?? throw new System.NullReferenceException(),
            Email = author.Email ?? throw new System.NullReferenceException(),
        };
    }

    public Task<string?> GetText(Guid id)
    {
        return context.Cheep
            .Where(c => c.Id == id.ToString())
            .Select(c => c.Text)
            .FirstOrDefaultAsync();
    }

    public Task<ulong?> GetTimestamp(Guid id)
    {
        return FirstOrElseAsync(
            context.Cheep
                .Where(c => c.Id == id.ToString())
                .Select(c => (ulong?) c.Timestamp),
            (ulong?) null
        );
    }

    // Async/await is unnesecary if you don't need the control flow to return back to the function after an await point. Here we simply return immediately after creating the task, so we shouldn't use await and turn this into an async function, to minimize overhead. https://stackoverflow.com/questions/38017016/async-task-then-await-task-vs-task-then-return-task
    // Cannot return null instead of generic type because valuetypes/structs aren't stored as references. https://stackoverflow.com/questions/302096/how-can-i-return-null-from-a-generic-method-in-c
    internal static Task<T> FirstOrElseAsync<T>(IQueryable<T> query, T def)
    {
        try
        {
            return query.FirstAsync();
        }
        catch (InvalidOperationException)
        {
            return Task.FromResult(def);
        }
    }

    public async Task<bool> Put(CheepDTO cheep)
    {
        var author = await context.Author.Where(a => a.UserName == cheep.Author).FirstOrDefaultAsync();
        if (author is null) return false;

        await context.Cheep.AddAsync(new Cheep
           {
                Author = author,
                Text = cheep.Text,
                Timestamp = checked ((long) cheep.Timestamp),
            });

        await context.SaveChangesAsync();
        return true;
    }
}