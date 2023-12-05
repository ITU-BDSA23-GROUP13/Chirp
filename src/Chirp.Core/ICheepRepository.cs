namespace Chirp.Core;

public interface ICheepRepository
{
    public enum Order
    {
        Newest,
        Oldest,
    }

    /// <summary>
    ///     Gets a single page from all cheeps sorted by the given order.
    /// </summary>
    public Task<IList<CheepDTO>> GetPageFromAll(uint page, uint pageSize, Order order = Order.Newest);

    /// <summary>
    ///     Gets the total count of cheeps.
    /// </summary>
    public Task<uint> GetAllCount();

    /// <summary>
    ///     Gets a single page from all the cheeps from all the authors the given user follows
    ///     sorted by the given order.
    /// </summary>
    public Task<IList<CheepDTO>?> GetPageFromFollowed(string user, uint page, uint pageSize, Order order = Order.Newest);

    /// <summary>
    ///     Gets the total count of cheeps from all the authors the given user follows.
    /// </summary>
    public Task<uint?> GetFollowedCount(string user);

    public Task<CheepDTO?> Get(Guid id);

    public Task<AuthorDTO?> GetAuthor(Guid id);

    public Task<string?> GetText(Guid id);

    public Task<ulong?> GetTimestamp(Guid id);

    public Task<bool> Put(CheepDTO cheep);
}