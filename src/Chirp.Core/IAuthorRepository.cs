using static Chirp.Core.ICheepRepository;

namespace Chirp.Core;

public interface IAuthorRepository {

    public Task<AuthorDTO?> Get(string name);

    public Task<string?> GetEmail(string name);

    public Task<IList<CheepDTO>> GetCheepsPageSortedBy(string name, uint page, uint pageSize, Order order = Order.Newest);

    public Task<IReadOnlyCollection<CheepDTO>?> GetCheeps(string name);

}