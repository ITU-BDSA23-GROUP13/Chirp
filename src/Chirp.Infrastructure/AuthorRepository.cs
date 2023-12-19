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

    public async Task<uint> GetCount()
    {
        return (uint) await context.Author.CountAsync();
    }

    /// <summary>
    /// Returns null, we no author was found.
    /// Throws NullReferenceException if the author was found, but the name or email was null.
    /// </summary>
    public async Task<AuthorDTO?> Get(string name)
    {
        var author = await context.Author
            .Where(a => a.UserName == name)
            .FirstOrDefaultAsync();
        if (author is null) return null;

        return new AuthorDTO
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

    public async Task<IList<CheepDTO>?> GetCheepsPage(string name, uint page, uint pageSize, Order order)
    {
        var author = await context.Author
            .Where(a => a.UserName == name)
            .Include(a => a.Cheeps)
            .FirstOrDefaultAsync();
        if (author is null) return null;

        var cheeps = order switch
        {
            Order.Newest => author.Cheeps.OrderByDescending(c => c.Timestamp),
            Order.Oldest => author.Cheeps.OrderBy(c => c.Timestamp),
            _ => throw new System.Diagnostics.UnreachableException(),
        };

        var list = cheeps
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

    public async Task<IReadOnlyCollection<string>?> GetFollowed(string author)
    {
        if (!await context.Author.AnyAsync(a => a.UserName == author)) return null;

        return context.Author
            .SelectMany(a => a.Followed, (followee, follower) => new {followee, follower})
            .Where(f => f.follower.UserName == author)
            .Select(f => f.followee.UserName!)
            .ToList();
    }

    public async Task<bool?> GetFollowing(string followerName, string followeeName)
    {
        if (!await context.Author.AnyAsync(a => a.UserName == followerName)) return null;
        if (!await context.Author.AnyAsync(a => a.UserName == followeeName)) return null;

        var a = await context.Author
            .SelectMany(a => a.Followed, (followee, follower) => new {followee, follower})
            .AnyAsync(f => f.followee.UserName == followeeName && f.follower.UserName == followerName);
        if (a) return true;

        var b = await context.Author
            .SelectMany(a => a.Followers, (follower, followee) => new {follower, followee})
            .AnyAsync(f => f.follower.UserName == followerName && f.followee.UserName == followeeName);
        if (b) return true;

        return false;
    }

    public async Task<bool> PutFollowing(string followerName, string followeeName)
    {
        var follower = await context.Author.Include(a => a.Followed).Where(c => c.UserName == followerName).FirstOrDefaultAsync();
        if (follower is null) return false;

        var followee = await context.Author.Include(a => a.Followers).Where(c => c.UserName == followeeName).FirstOrDefaultAsync();
        if (followee is null) return false;

        if (follower.Followed.Any(a => a.UserName == followeeName)) return false;
        if (followee.Followers.Any(a => a.UserName == followerName)) return false;

        follower.Followers.Add(followee);
        followee.Followed.Add(follower);

        // context.Author.Update(author1);
        // context.Author.Update(author2);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFollowing(string followerName, string followeeName)
    {
        var follower = await context.Author.Include(a => a.Followed).Where(c => c.UserName == followerName).FirstOrDefaultAsync();
        if (follower is null) return false;

        var followee = await context.Author.Include(a => a.Followed).Where(c => c.UserName == followeeName).FirstOrDefaultAsync();
        if (followee is null) return false;

        // Uses single & to both Remove methods are called.
        if (!follower.Followers.Remove(followee) & !followee.Followed.Remove(follower)) return false;

        // context.Author.Update(author1);
        // context.Author.Update(author2);

        await context.SaveChangesAsync();
        return true;
    }

}