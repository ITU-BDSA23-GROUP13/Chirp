using Chirp.Core;

namespace Chirp.Infrastructure;

public class ChirpRepository : IChirpRepository
{
    private CheepContext context = new();

    public void CreateAuthor(Guid id, AuthorDTO author)
    {
        throw new NotImplementedException();
    }

    public void CreateCheep(Guid id, CheepDTO cheepDTO)
    {
        var cheep = new Cheep
        {
            AuthorName = cheepDTO.Author,
            Text = cheepDTO.Text,
            Timestamp = cheepDTO.Timestamp,
        };
            
        context.Add(cheep);
    }

    public void DeleteAuthor(Guid id)
    {
        throw new NotImplementedException();
    }

    public void DeleteCheep(Guid id)
    {
        throw new NotImplementedException();
    }

    public AuthorDTO ReadAuthor(Guid id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<AuthorDTO> ReadAuthors()
    {
        throw new NotImplementedException();
    }

    public CheepDTO ReadCheep(Guid id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<CheepDTO> ReadCheeps()
    {
        throw new NotImplementedException();
    }

    public void UpdateAuthor(AuthorDTO author)
    {
        throw new NotImplementedException();
    }

    public void UpdateCheep(CheepDTO cheep)
    {
        throw new NotImplementedException();
    }
}