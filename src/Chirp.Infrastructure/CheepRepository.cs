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

        return await cheeps
            .Skip((int) ((page - 1) * pageSize))
            .Take((int) pageSize)
            .Select(c =>
                new CheepDTO
                {
                    Author = c.Author.Name,
                    Text = c.Text,
                    Timestamp = (ulong) c.Timestamp,
                }
            )
            .ToListAsync();
    }

    public async Task<uint> GetCount()
    {
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/checked-and-unchecked
        return checked ((uint) await this.context.Cheep.CountAsync());
    }

    public async Task<CheepDTO?> Get(ulong id)
    {
        Cheep? cheep = await context.Cheep
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        return cheep is null ? null : new CheepDTO
        {
            Author = cheep.Author.Name,
            Text = cheep.Text,
            Timestamp = (ulong) cheep.Timestamp,
        };
    }

    public async Task<AuthorDTO?> GetAuthor(ulong id)
    {
        Author? author = await context.Cheep
            .Where(c => c.Id == id)
            .Select(c => c.Author)
            .FirstOrDefaultAsync();

        return author is null ? null : new AuthorDTO
        {
            Name = author.Name,
            Email = author.Email,
        };
    }

    public Task<string?> GetText(ulong id)
    {
        return context.Cheep
            .Where(c => c.Id == id)
            .Select(c => c.Text)
            .FirstOrDefaultAsync();
    }

    public Task<ulong?> GetTimestamp(ulong id)
    {
        return FirstOrElseAsync(
            context.Cheep
                .Where(c => c.Id == id)
                .Select(c => (ulong?) c.Timestamp),
            (ulong?) null
        );
    }

    // Async/await is unnesecary if you don't need to be to return back to the function after an await point. Here we simply return immediately after awaiting. https://stackoverflow.com/questions/38017016/async-task-then-await-task-vs-task-then-return-task
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
}