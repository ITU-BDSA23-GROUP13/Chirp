using Chirp.Core;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, DateTimeOffset Timestamp);

public interface ICheepService
{
    public Task<List<CheepViewModel>> GetCheeps(uint page);
    public Task<List<CheepViewModel>> GetCheepsFromAuthor(string author, uint page);
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

    public async Task<List<CheepViewModel>> GetCheeps(uint page)
    {
        var cheeps = await cheepRepository.GetPageSortedBy(page, pageSize);
        return cheeps
            .Select(c => new CheepViewModel(c.Author, c.Text, DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)))
            .ToList();
    }

    public async Task<List<CheepViewModel>> GetCheepsFromAuthor(string author, uint page)
    {
        var cheeps = await authorRepository.GetCheepsPageSortedBy(author, page, pageSize);
        return cheeps
            .Select(c => new CheepViewModel(c.Author, c.Text, DateTimeOffset.FromUnixTimeSeconds((long) c.Timestamp)))
            .ToList();
    }
}