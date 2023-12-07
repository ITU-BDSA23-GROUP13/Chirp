using static Chirp.Core.ICheepRepository;

namespace Chirp.Core;

/// <summary>
/// Contains method used to execute read or write queries on the database.
/// In case of errors, either an exception is thrown or
/// null (for read queries) or false (for write queries) is returned.
/// In general a null or false value indicates that the query failed
/// because of invalid input. (e.g. a user with the given name did not exist)
/// And an exception indicates that the query failed because of an internal error,
/// and means that something went wrong in the system.
/// </summary>
public interface IAuthorRepository {

    public Task<uint> GetCount();

    public Task<AuthorDTO?> Get(string name);

    /// <summary>
    /// Returns null, if no author with the given name was found.
    /// Throws NullReferenceException if the author was found, but the name was null.
    /// </summary>
    public Task<IReadOnlyCollection<CheepDTO>?> GetCheeps(string name);

    public Task<string?> GetEmail(string name);

    /// <summary>
    /// Returns null, if no author with the given name was found.
    /// Throws NullReferenceException if the author was found, but the name was null.
    /// </summary>
    public Task<IList<CheepDTO>?> GetCheepsPage(string name, uint page, uint pageSize, Order order = Order.Newest);

    /// <summary>
    /// Returns null, if no author with the given name was found.
    /// </summary>
    public Task<uint?> GetCheepCount(string name);

    public Task<bool> Put(AuthorDTO author);

    public Task<bool?> GetFollowing(string follower, string followee);

    /// <summary>
    /// Returns true, if successful.
    /// Returns false, if either user did not exist or the author was already followed.
    /// </summary>
    public Task<bool> PutFollowing(string follower, string followee);

    /// <summary>
    /// Returns true, if successful.
    /// Returns false, if either user did not exist or the author was not followed.
    /// </summary>
    public Task<bool> DeleteFollowing(string follower, string followee);

}