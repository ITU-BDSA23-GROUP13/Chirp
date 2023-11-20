using Chirp.Core;

namespace Chirp.Web;

public record CheepViewModel(string Author, string Message, DateTimeOffset Timestamp);

public interface ICheepService
{
    public Task<(List<CheepViewModel>,uint)> GetCheepsAndTotalCount(uint page);
    public Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromAuthor(string author, uint page);
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

    public async Task<(List<CheepViewModel>,uint)> GetCheepsAndTotalCount(uint page)
    {
        var cheeps = await cheepRepository.GetPageSortedBy(page, pageSize);

        var list = cheeps
            .Select(c => new CheepViewModel(c.Author, c.Text, DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)))
            .ToList();

        var totalCount = await cheepRepository.GetCount();

        return (list, totalCount);
    }

    public async Task<(List<CheepViewModel>,uint)?> GetCheepsAndTotalCountFromAuthor(string author, uint page)
    {
        var cheeps = await authorRepository.GetCheepsPageSortedBy(author, page, pageSize);
        if (cheeps is null) return null;

        var list = cheeps
            .Select(c => new CheepViewModel(c.Author, c.Text, DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)))
            .ToList();

        var totalCount = await authorRepository.GetCheepCount(author);
        if (totalCount is null) return null;

        return (list, (uint) totalCount);
    }
}