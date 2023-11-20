using Chirp.Core;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

using static Chirp.Core.ICheepRepository;

public class AuthorRepository : IAuthorRepository
{
    // Chirp.Core.Author extends IdentityUser which contains nullable attributes.
    // There attributes are not supposed to be null, and any data will be null checked
    // before returning.

    private readonly ChirpContext context;

    public AuthorRepository(ChirpContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Returns null, we no author was found.
    /// Throws NullReferenceException if the author was found, but the name or email was null.
    /// </summary>
    public async Task<AuthorDTO?> Get(string name)
    {
        Author? author = await context.Author
            .Where(a => a.UserName == name)
            .FirstOrDefaultAsync();

        return author is null ? null : new AuthorDTO
        {
            Name = author.UserName ?? throw new System.NullReferenceException(),
            Email = author.Email ?? throw new System.NullReferenceException(),
        };
    }

    /// <summary>
    /// Returns null, we no author was found.
    /// Throws NullReferenceException if the author was found, but the email was null.
    /// </summary>
    public async Task<string?> GetEmail(string name)
    {
        Author? author = await context.Author
            .Where(a => a.UserName == name)
            .FirstOrDefaultAsync();
        return author?.Email ?? throw new System.NullReferenceException();
    }

    /// <summary>
    /// Returns null, we no author was found.
    /// Throws NullReferenceException if the author was found, but the name was null.
    /// </summary>
    public async Task<IReadOnlyCollection<CheepDTO>?> GetCheeps(string name)
    {
        Author? author = await context.Author
            .Where(a => a.UserName == name)
            .FirstOrDefaultAsync();

        return author?.Cheeps
            .Select(cheep =>
                new CheepDTO
                {
                    Author = author.UserName ?? throw new System.NullReferenceException(),
                    Text = cheep.Text,
                    Timestamp = (ulong) cheep.Timestamp,
                })
            .ToList();
    }

    /// <summary>
    /// Returns null, we no author was found.
    /// Throws NullReferenceException if the author was found, but the name was null.
    /// </summary>
    public async Task<IList<CheepDTO>> GetCheepsPageSortedBy(string name, uint page, uint pageSize, Order order)
    {
        var cheeps = order switch
        {
            Order.Newest => context.Cheep.OrderByDescending(c => c.Timestamp),
            Order.Oldest => context.Cheep.OrderBy(c => c.Timestamp),
            _ => throw new System.Diagnostics.UnreachableException(),
        };

        var list = await cheeps
            .Where(c => c.Author.UserName == name)
            .Skip((int) ((page - 1) * pageSize))
            .Take((int) pageSize)
            .Select(c =>
                new CheepDTO
                {
                    Author = c.Author.UserName!, // We aren't allowed to throw exceptions here
                    Text = c.Text,
                    Timestamp = (ulong) c.Timestamp,
                }
            )
            .ToListAsync();

        // ... so we check here instead
        if (list.Any(c => c.Author is null))
            throw new System.Diagnostics.UnreachableException();

        return list;
    }

    public async Task<uint?> GetCheepCount(string name)
    {
        var cheeps = await context.Author
            .Where(a => a.UserName == name)
            .Select(a => a.Cheeps)
            .FirstOrDefaultAsync();

        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/checked-and-unchecked
        return checked ((uint?) cheeps?.Count);
    }

}