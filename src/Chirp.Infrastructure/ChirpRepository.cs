using Chirp.Core;

namespace Chirp.Infrastructure;

public class ChirpRepository : IChirpRepository
{
    private static ChirpContext context = new();

    public void CreateAuthor(Guid id, AuthorDTO authorDTO)
    {
        var author = new Author
        {
            Id = id.ToString(),
            Name = authorDTO.Name,
            Email = authorDTO.Email,
        };
        context.Add(author);
    }

    public void CreateCheep(Guid id, CheepDTO cheepDTO)
    {
        var cheep = new Cheep
        {
            Id = id.ToString(),
            AuthorId = GetAuthorIdFromName(cheepDTO.Author.Name).ToString(),
            Text = cheepDTO.Text,
            Timestamp = cheepDTO.Timestamp.ToUnixTimeSeconds(),
        };
        context.Add(cheep);
    }

    public IReadOnlyCollection<AuthorDTO> ReadAuthors()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<CheepDTO> ReadCheeps()
    {
        var cheeps = context.Cheeps.Select(c =>
            new CheepDTO
            {
                Author = ReadAuthor(Guid.Parse(c.AuthorId)),
                Text = c.Text,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(c.Timestamp),
            }
        );
        return cheeps.ToList();
    }

    public IReadOnlyCollection<CheepDTO> ReadCheepsFromAuthor(Guid authorId)
    {
        throw new NotImplementedException();
    }

    public CheepDTO ReadCheep(Guid id)
    {
        throw new NotImplementedException();
    }

    public AuthorDTO ReadAuthor(Guid id)
    {
        var author = context.Authors.Where(a => a.Id == id.ToString()).First();
        return new AuthorDTO
        {
            Name = author.Name,
            Email = author.Email,
        };
    }

    public Guid GetAuthorIdFromName(string name)
    {
        var author = context.Authors.Where(a => a.Name == name).First();
        return Guid.Parse(author.Id);
    }

    public void UpdateAuthor(AuthorDTO author)
    {
        throw new NotImplementedException();
    }

    public void UpdateCheep(CheepDTO cheep)
    {
        throw new NotImplementedException();
    }

    public void DeleteCheep(Guid id)
    {
        throw new NotImplementedException();
    }

    public void DeleteAuthor(Guid id)
    {
        throw new NotImplementedException();
    }
}