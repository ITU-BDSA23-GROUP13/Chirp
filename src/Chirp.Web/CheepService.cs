using Chirp.Core;

namespace Chirp.Web;

public class CheepViewModel
{
    public required string Author  { get; set; }
    public required string Message { get; set; }
    public required DateTimeOffset Timestamp { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not CheepViewModel cheep) return false;
        return cheep.Author == Author && cheep.Message == Message && cheep.Timestamp == Timestamp;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Author, Message, Timestamp);
    }
}

public interface ICheepService
{
    /// <summary>
    /// The <paramref name="page"/> parameter is 1-indexed.
    /// </summary>
    public Task<(List<CheepViewModel>,uint)> GetCheepsAndPageCount(uint page);

    /// <summary>
    /// The <paramref name="page"/> parameter is 1-indexed.
    /// </summary>
    public Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromAuthor(string author, uint page);

    /// <summary>
    /// The <paramref name="page"/> parameter is 1-indexed.
    /// </summary>
    public Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromFollowed(string user, uint page);

    public Task<uint?> GetCheepCount(string author);
    public Task<bool> PutCheep(CheepViewModel cheep);

    public Task<IReadOnlyCollection<string>?> GetFollowsAll(string author);
    public Task<uint?> GetFollowerCount(string author);

    public Task<bool?> GetFollow(string follower, string followee);
    public Task<bool> PutFollowing(string follower, string followee);
    public Task<bool> DeleteFollowing(string follower, string followee);

    public Task<bool> DeleteAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly uint pageSize = 32;
    private readonly ICheepRepository cheepRepository;
    private readonly IAuthorRepository authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        this.cheepRepository = cheepRepository;
        this.authorRepository = authorRepository;
    }

    public async Task<(List<CheepViewModel>,uint)> GetCheepsAndPageCount(uint page)
    {
        if (page == 0) throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0");

        var cheeps = await cheepRepository.GetPageFromAll(page, pageSize);

        var list = cheeps
            .Select(c => new CheepViewModel
                {
                    Author = c.Author,
                    Message = c.Text,
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)
                })
            .ToList();

        var totalCount = await cheepRepository.GetAllCount();

        return (list, (uint) Math.Ceiling((decimal) totalCount / (decimal) pageSize));
    }

    public async Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromAuthor(string author, uint page)
    {
        if (page == 0) throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0");

        var authorDTO = await authorRepository.Get(author);

        var cheeps = await authorRepository.GetCheepsPage(author, page, pageSize);
        if (cheeps is null) return null;

        var list = cheeps
            .Select(c => new CheepViewModel
                {
                    Author = c.Author,
                    Message = c.Text,
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)
                })
            .ToList();

        var totalCount = await authorRepository.GetCheepCount(author);
        if (totalCount is null) return null;

        return (list, (uint) Math.Ceiling((decimal) totalCount / (decimal) pageSize));
    }

    public async Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromFollowed(string author, uint page)
    {
        if (page == 0) throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0");

        var cheeps = await cheepRepository.GetPageFromFollowed(author, page, pageSize);
        if (cheeps is null) return null;

        var list = cheeps
            .Select(c => new CheepViewModel
                {
                    Author = c.Author,
                    Message = c.Text,
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)
                })
            .ToList();

        var totalCount = await cheepRepository.GetFollowedCount(author);
        if (cheeps is null) return null;

        return (list, (uint) Math.Ceiling((decimal) totalCount / (decimal) pageSize));
    }

    public Task<uint?> GetCheepCount(string author) =>
        authorRepository.GetCheepCount(author);

    public Task<bool> PutCheep(CheepViewModel cheep) =>
        cheepRepository.Put(new CheepDTO
        {
            Author = cheep.Author,
            Text = cheep.Message,
            Timestamp = (ulong) cheep.Timestamp.ToUnixTimeSeconds(),
        });

    public Task<IReadOnlyCollection<string>?> GetFollowsAll(string author) =>
        authorRepository.GetFollowed(author);

    public Task<uint?> GetFollowerCount(string user) =>
        authorRepository.GetFollowerCount(user);

    public Task<bool?> GetFollow(string follower, string followee) =>
        authorRepository.GetFollowing(follower, followee);

    public Task<bool> PutFollowing(string follower, string followee) =>
        authorRepository.PutFollowing(follower, followee);

    public Task<bool> DeleteFollowing(string follower, string followee) =>
        authorRepository.DeleteFollowing(follower, followee);

    public Task<bool> DeleteAuthor(string author) =>
        authorRepository.DeleteAuthor(author);
}