using Chirp.Core;

namespace Chirp.Web;

public class CheepViewModel
{
    public required string Author  { get; set; }
    public required string Message { get; set; }
    public required DateTimeOffset Timestamp { get; set; }
}

public interface ICheepService
{
    public Task<(List<CheepViewModel>,uint)> GetCheepsAndPageCount(uint page);
    public Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromAuthor(string author, uint page);
    public Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromFollowed(string user, uint page);
    public Task<bool> PutCheep(CheepViewModel cheep);
    public Task<bool> FollowAuthor(string user, string author);
    public Task<bool> UnfollowAuthor(string user, string author);
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

        return (list, (uint) totalCount);
    }

    public async Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromFollowed(string user, uint page)
    {
        var cheeps = await cheepRepository.GetPageFromFollowed(user, page, pageSize);
        if (cheeps is null) return null;

        var list = cheeps
            .Select(c => new CheepViewModel
                {
                    Author = c.Author,
                    Message = c.Text,
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)
                })
            .ToList();

        var totalCount = await cheepRepository.GetFollowedCount(user);
        if (cheeps is null) return null;

        return (list, (uint) Math.Ceiling((decimal) totalCount / (decimal) pageSize));
    }

    public Task<bool> PutCheep(CheepViewModel cheep)
    {
        return cheepRepository.Put(new CheepDTO
        {
            Author = cheep.Author,
            Text = cheep.Message,
            Timestamp = (ulong) cheep.Timestamp.ToUnixTimeSeconds(),
        });
    }

    public Task<bool> FollowAuthor(string user, string author)
    {
        return authorRepository.PutFollowing(user, author);
    }

    public Task<bool> UnfollowAuthor(string user, string author)
    {
        return authorRepository.DeleteFollowing(user, author);
    }

}