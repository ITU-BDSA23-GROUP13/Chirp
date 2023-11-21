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

    public async Task<IList<CheepDTO>> GetPageSortedBy(uint page, uint pageSize, Order order)
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

    public async Task<uint> GetCount()
    {
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/checked-and-unchecked
        // We want this to throw an exception if the cast fails.
        return checked ((uint) await this.context.Cheep.CountAsync());
    }

    public async Task<CheepDTO?> Get(Guid id)
    {
        Cheep? cheep = await context.Cheep
            .Where(c => c.Id == id.ToString())
            .FirstOrDefaultAsync();

        return cheep is null ? null : new CheepDTO
        {
            Author = cheep.Author.UserName ?? throw new System.NullReferenceException(),
            Text = cheep.Text,
            Timestamp = (ulong) cheep.Timestamp,
        };
    }

    public async Task<AuthorDTO?> GetAuthor(Guid id)
    {
        Author? author = await context.Cheep
            .Where(c => c.Id == id.ToString())
            .Select(c => c.Author)
            .FirstOrDefaultAsync();

        return author is null ? null : new AuthorDTO
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

    public async Task Put(CheepDTO cheep)
    {
        var author = await context.Author.Where(a => a.UserName == cheep.Author).FirstOrDefaultAsync();

        if (author is null)
            throw new InvalidOperationException("Author does not exist.");

        await context.Cheep.AddAsync(new Cheep
           {
                Author = author,
                Text = cheep.Text,
                Timestamp = checked ((long) cheep.Timestamp),
            });
    }
}