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
                Author = context.Author
                    .Where(a => a.Id == c.AuthorId)
                    .Select(a => a.Name)
                    .First(),
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

    //public async Task<AuthorDTO> ReadAuthor(ulong id)
    //{
    //    var author = await context.Author.Where(a => a.Id == id).FirstAsync();
    //    return new AuthorDTO
    //    {
    //        Name = author.Name,
    //        Email = author.Email,
    //    };
    //}

    public Task<ulong> GetAuthorIdFromName(string name)
    {
        var author = context.Author.Where(a => a.Name == name).First();
        return Task.Run(() => author.Id);
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