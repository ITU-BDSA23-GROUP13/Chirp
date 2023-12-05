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
        var author = await context.Author
            .Where(a => a.UserName == name)
            .FirstOrDefaultAsync();
        if (author is null) return null;
        return author.Email ?? throw new System.NullReferenceException();
    }

    public async Task<IReadOnlyCollection<CheepDTO>?> GetCheeps(string name)
    {
        var author = await context.Author
            .Where(a => a.UserName == name)
            .FirstOrDefaultAsync();
        if (author is null) return null;

        return author.Cheeps
            .Select(cheep =>
                new CheepDTO
                {
                    Author = author.UserName ?? throw new System.NullReferenceException(),
                    Text = cheep.Text,
                    Timestamp = (ulong) cheep.Timestamp,
                })
            .ToList();
    }

    public async Task<IList<CheepDTO>?> GetCheepsPageSortedBy(string name, uint page, uint pageSize, Order order)
    {
        var author = await context.Author
            .Where(a => a.UserName == name)
            .FirstOrDefaultAsync();
        if (author is null) return null;

        var cheeps = order switch
        {
            Order.Newest => author.Cheeps.OrderByDescending(c => c.Timestamp),
            Order.Oldest => author.Cheeps.OrderBy(c => c.Timestamp),
            _ => throw new System.Diagnostics.UnreachableException(),
        };

        var list = cheeps
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
            .ToList();

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

    public async Task<bool> Put(AuthorDTO author)
    {
        await context.Author.AddAsync(new Author
            {
                UserName = author.Name,
                Email = author.Email,
            });

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> PutFollowing(string follower, string followee)
    {
        var author1 = await context.Author.Where(c => c.UserName == follower).FirstOrDefaultAsync();
        if (author1 is null) return false;

        var author2 = await context.Author.Where(c => c.UserName == followee).FirstOrDefaultAsync();
        if (author2 is null) return false;

        if (author1.Followers.Any(a => a.UserName == followee)) return false;
        if (author2.Followed.Any(a => a.UserName == follower)) return false;

        author1.Followers.Add(author2);
        author2.Followed.Add(author1);

        // context.Author.Update(author1);
        // context.Author.Update(author2);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFollowing(string follower, string followee)
    {
        var author1 = await context.Author.Where(c => c.UserName == follower).FirstOrDefaultAsync();
        if (author1 is null) return false;

        var author2 = await context.Author.Where(c => c.UserName == followee).FirstOrDefaultAsync();
        if (author2 is null) return false;

        if (!author1.Followers.Remove(author2)) return false;
        if (!author2.Followed.Remove(author1)) return false;

        // context.Author.Update(author1);
        // context.Author.Update(author2);

        await context.SaveChangesAsync();
        return true;
    }

}