namespace Chirp.Core;

public interface ICheepRepository
{
    public enum Order
    {
        Newest,
        Oldest,
    }

    public Task<IList<CheepDTO>> GetPageSortedBy(uint page, uint pageSize, Order order = Order.Newest);
    public Task<uint> GetCount();

    public Task<CheepDTO?> Get(Guid id);

    public Task<AuthorDTO?> GetAuthor(Guid id);

    public Task<string?> GetText(Guid id);

    public Task<ulong?> GetTimestamp(Guid id);

    public Task Put(CheepDTO cheep);
}