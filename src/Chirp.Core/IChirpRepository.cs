namespace Chirp.Core;

public interface IChirpRepository
{

    //Task CreateCheep(Guid id, CheepDTO cheep);
    //Task CreateAuthor(Guid id, AuthorDTO author);

    Task<IReadOnlyCollection<CheepDTO>> ReadCheeps();
    //Task<IReadOnlyCollection<AuthorDTO>> ReadAuthors();

    //int ReadNumberOfCheeps();
    //int ReadNumberOfPagesOfCheeps();

    Task<IReadOnlyCollection<CheepDTO>> ReadCheepsFromAuthor(ulong authorId);

    //Task<CheepDTO> ReadCheep(Guid id);
    //Task<AuthorDTO> ReadAuthor(Guid id);

    Task<ulong> GetAuthorIdFromName(string name);

    //Task UpdateCheep(CheepDTO cheep);
    //Task UpdateAuthor(AuthorDTO author);

    //Task DeleteCheep(Guid id);
    //Task DeleteAuthor(Guid id);

}