namespace Chirp.Core;

public interface IChirpRepository {

    void CreateCheep(Guid id, CheepDTO cheep);
    void CreateAuthor(Guid id, AuthorDTO author);

    IReadOnlyCollection<CheepDTO> ReadCheeps();
    IReadOnlyCollection<AuthorDTO> ReadAuthors();

    IReadOnlyCollection<CheepDTO> ReadCheepsFromAuthor(Guid authorId);

    CheepDTO ReadCheep(Guid id);
    AuthorDTO ReadAuthor(Guid id);

    Guid GetAuthorIdFromName(string name);

    void UpdateCheep(CheepDTO cheep);
    void UpdateAuthor(AuthorDTO author);

    void DeleteCheep(Guid id);
    void DeleteAuthor(Guid id);

}