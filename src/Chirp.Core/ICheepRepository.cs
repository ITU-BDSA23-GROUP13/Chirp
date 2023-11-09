namespace Chirp.Core;

public interface ICheepRepository
{
    public enum Order
    {
        Newest,
        Oldest,
    }

    public Task<IList<CheepDTO>> GetPageSortedBy(uint page, uint pageSize, Order order = Order.Newest);

    public Task<CheepDTO?> Get(ulong id);

    public Task<AuthorDTO?> GetAuthor(ulong id);

    public Task<string?> GetText(ulong id);

    public Task<ulong?> GetTimestamp(ulong id);

}