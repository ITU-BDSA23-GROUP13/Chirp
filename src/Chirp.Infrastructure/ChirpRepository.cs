using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

public class ChirpRepository : IChirpRepository
{
    private readonly ChirpContext context = new();

    //public async Task CreateCheep(Guid id, CheepDTO cheepDTO)
    //{
    //    var authorId = await GetAuthorIdFromName(cheepDTO.Author.Name);
    //    var cheep = new Cheep
    //    {
    //        Id = id.ToString(),
    //        AuthorId = authorId.ToString(),
    //        Text = cheepDTO.Text,
    //        Timestamp = cheepDTO.Timestamp.ToUnixTimeSeconds(),
    //    };
    //    await context.AddAsync(cheep);
    //}

    //public async Task CreateAuthor(Guid id, AuthorDTO authorDTO)
    //{
    //    var author = new Author
    //    {
    //        Id = id.ToString(),
    //        Name = authorDTO.Name,
    //        Email = authorDTO.Email,
    //    };
    //    await context.AddAsync(author);
    //}

    public async Task<IReadOnlyCollection<CheepDTO>> ReadCheeps()
    {
        return await context.Cheep.Select(c =>
            new CheepDTO
            {
                Author = "Test",//await ReadAuthor(Guid.Parse(c.AuthorId));
                Text = c.Text,
                Timestamp = c.Timestamp,
            }
        )
        .ToListAsync();
    }

    //public Task<IReadOnlyCollection<AuthorDTO>> ReadAuthors()
    //{
    //    throw new NotImplementedException();
    //}

    public Task<IReadOnlyCollection<CheepDTO>> ReadCheepsFromAuthor(ulong authorId)
    {
        throw new NotImplementedException();
    }

    //public Task<CheepDTO> ReadCheep(Guid id)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<AuthorDTO> ReadAuthor(Guid id)
    //{
    //    throw new NotImplementedException();
    //}

    public Task<ulong> GetAuthorIdFromName(string name)
    {
        var author = context.Author.Where(a => a.Name == name).First();
        return Task.Run(() => author.Id);//() => Guid.Parse(author.Id));
    }

    //public Task UpdateAuthor(AuthorDTO author)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task UpdateCheep(CheepDTO cheep)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task DeleteCheep(Guid id)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task DeleteAuthor(Guid id)
    //{
    //    throw new NotImplementedException();
    //}
}