using Chirp.Core;
using Chirp.Infrastructure;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, DateTimeOffset Timestamp);

public interface ICheepService
{
    public Task<List<CheepViewModel>> GetCheeps(int page);
    public Task<List<CheepViewModel>> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private int pageSize = 32;
    private IChirpRepository chirpRepository = new ChirpRepository();

    public async Task<List<CheepViewModel>> GetCheeps(int page)
    {
        var cheeps = await chirpRepository.ReadCheeps();
        return cheeps
            .OrderByDescending(cheep => cheep.Timestamp)
            .Skip((page-1) * pageSize)
            .Take(pageSize)
            .Select(c => new CheepViewModel(c.Author, c.Text, DateTimeOffset.FromUnixTimeSeconds(c.Timestamp)))
            .ToList();
    }

    public async Task<List<CheepViewModel>> GetCheepsFromAuthor(string author)
    {
        try
        {
            var authorId = await chirpRepository.GetAuthorIdFromName(author);
            var cheeps = await chirpRepository.ReadCheepsFromAuthor(authorId);
            return cheeps.Select(c => new CheepViewModel(c.Author, c.Text, DateTimeOffset.FromUnixTimeSeconds(c.Timestamp))).ToList();
        }
        catch
        {
            return new List<CheepViewModel>();
        }
    }
}