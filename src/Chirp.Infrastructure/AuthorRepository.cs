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
    /// Gets the author with the given name.
    /// Returns null if we no author was found.
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
    /// Gets the email of the author with the given name.
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

    /// <summary>
    /// Gets all the cheeps from the author with the given name.
    /// Returns null, we no author was found.
    /// Throws NullReferenceException if the author was found, but the name was null.
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

    /// <summary>
    /// Returns a page of cheeps from the author with the given name, ordered by timestamp.
    /// The <paramref name="page"/> parameter is 0-indexed.
    /// </summary>
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

    /// <summary>
    /// Returns the total number of cheeps from the author with the given name.
    /// </summary>
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
            .SelectMany(a => a.Follows, (follower, followee) => new {follower, followee})
            .Where(f => f.follower.UserName == author)
            .Select(f => f.followee.UserName!)
            .ToList();
    }

    public async Task<uint?> GetFollowerCount(string author)
    {
        if (!await context.Author.AnyAsync(a => a.UserName == author)) return null;

        return checked ((uint?) await context.Author
            .SelectMany(a => a.FollowedBy, (followee, follower) => new {followee, follower})
            .Where(f => f.followee.UserName == author)
            .CountAsync());
    }

    public async Task<bool?> GetFollowing(string followerName, string followeeName)
    {
        if (!await context.Author.AnyAsync(a => a.UserName == followerName)) return null;
        if (!await context.Author.AnyAsync(a => a.UserName == followeeName)) return null;

        var followerFollowsFollowee = await context.Author
            .SelectMany(a => a.Follows, (follower, followee) => new {follower, followee})
            .AnyAsync(f => f.follower.UserName == followerName && f.followee.UserName == followeeName);
        if (followerFollowsFollowee) return true;

        var followeeFollowedByFollower = await context.Author
            .SelectMany(a => a.FollowedBy, (followee, follower) => new {followee, follower})
            .AnyAsync(f => f.followee.UserName == followeeName && f.follower.UserName == followerName);
        if (followeeFollowedByFollower) return true;

        return false;
    }

    public async Task<bool> PutFollowing(string followerName, string followeeName)
    {
        var follower = await context.Author.Include(a => a.Follows).Where(c => c.UserName == followerName).FirstOrDefaultAsync();
        if (follower is null) return false;

        var followee = await context.Author.Include(a => a.FollowedBy).Where(c => c.UserName == followeeName).FirstOrDefaultAsync();
        if (followee is null) return false;

        if (follower.Follows.Any(a => a.UserName == followeeName)) return false;
        
        if (followee.FollowedBy.Any(a => a.UserName == followerName)) return false;

        follower.Follows.Add(followee);
        followee.FollowedBy.Add(follower);

        // context.Author.Update(author1);
        // context.Author.Update(author2);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFollowing(string followerName, string followeeName)
    {
        var follower = await context.Author.Include(a => a.Follows).Where(c => c.UserName == followerName).FirstOrDefaultAsync();
        if (follower is null) return false;

        var followee = await context.Author.Include(a => a.Follows).Where(c => c.UserName == followeeName).FirstOrDefaultAsync();
        if (followee is null) return false;

        // Uses single & to both Remove methods are called.
        if (!follower.Follows.Remove(followee) & !followee.FollowedBy.Remove(follower)) return false;

        // context.Author.Update(author1);
        // context.Author.Update(author2);

        await context.SaveChangesAsync();
        return true;
    }

}