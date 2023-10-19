using Chirp.Core;
using Chirp.Infrastructure;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, DateTime Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private IChirpRepository chirpRepository = new ChirpRepository();

    public List<CheepViewModel> GetCheeps()
    {
        throw new NotImplementedException();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        var authorId = chirpRepository.GetAuthorIdFromName(author);
        var cheeps = chirpRepository.ReadCheepsFromAuthor(authorId);
        return cheeps.Select(c => new CheepViewModel(c.Author.Name, c.Text, c.Timestamp)).ToList();
    }
}