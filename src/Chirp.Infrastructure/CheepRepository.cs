using Chirp.Core;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

using static Chirp.Core.ICheepRepository;
using static RepositoryUtils;

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
        return (uint) await this.context.Cheep.CountAsync();
    }

    public async Task<CheepDTO?> Get(ulong id)
    {
        Cheep? cheep = await TryGetFirstAsyncElseNull(
            context.Cheep.Where(c => c.Id == id)
        );

        return cheep is null ? null : new CheepDTO
        {
            Author = cheep.Author.Name,
            Text = cheep.Text,
            Timestamp = (ulong) cheep.Timestamp,
        };
    }

    public async Task<AuthorDTO?> GetAuthor(ulong id)
    {
        Author? author = await TryGetFirstAsyncElseNull(
            context.Cheep.Where(c => c.Id == id).Select(c => c.Author)
        );

        return author is null ? null : new AuthorDTO
        {
            Name = author.Name,
            Email = author.Email,
        };
    }

    public Task<string?> GetText(ulong id)
    {
        return TryGetFirstAsyncElseNull(
            context.Cheep.Where(c => c.Id == id).Select(c => c.Text)
        );
    }

    public Task<ulong?> GetTimestamp(ulong id)
    {
        return TryGetFirstAsync(
            context.Cheep.Where(c => c.Id == id).Select(c => (ulong?) c.Timestamp),
            (ulong?) null
        );
    }
}