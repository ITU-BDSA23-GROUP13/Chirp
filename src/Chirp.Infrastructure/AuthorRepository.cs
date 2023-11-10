using Chirp.Core;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

using static Chirp.Core.ICheepRepository;

public class AuthorRepository : IAuthorRepository
{

    private readonly ChirpContext context;

    public AuthorRepository(ChirpContext context)
    {
        this.context = context;
    }

    public async Task<AuthorDTO?> Get(string name)
    {
        Author? author = await context.Author
            .Where(a => a.Name == name)
            .FirstOrDefaultAsync();

        return author is null ? null : new AuthorDTO
        {
            Name = author.Name,
            Email = author.Email,
        };
    }

    public async Task<string?> GetEmail(string name)
    {
        Author? author = await context.Author
            .Where(a => a.Name == name)
            .FirstOrDefaultAsync();
        return author?.Email;
    }

    public async Task<IReadOnlyCollection<CheepDTO>?> GetCheeps(string name)
    {
        Author? author = await context.Author
            .Where(a => a.Name == name)
            .FirstOrDefaultAsync();

        return author?.Cheeps
            .Select(cheep =>
                new CheepDTO
                {
                    Author = author.Name,
                    Text = cheep.Text,
                    Timestamp = (ulong) cheep.Timestamp,
                })
            .ToList();
    }

    public async Task<IList<CheepDTO>> GetCheepsPageSortedBy(string name, uint page, uint pageSize, Order order)
    {
        var cheeps = order switch
        {
            Order.Newest => context.Cheep.OrderByDescending(c => c.Timestamp),
            Order.Oldest => context.Cheep.OrderBy(c => c.Timestamp),
            _ => throw new System.Diagnostics.UnreachableException(),
        };

        return await cheeps
            .Where(c => c.Author.Name == name)
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

    public async Task<uint?> GetCheepCount(string name)
    {
        var cheeps = await context.Author
            .Where(a => a.Name == name)
            .Select(a => a.Cheeps)
            .FirstOrDefaultAsync();

        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/checked-and-unchecked
        return checked ((uint?) cheeps?.Count);
    }

}