namespace Chirp.Core;

public interface IRepository<T, K>
{
    void Create(K id, T entity);
    IReadOnlyCollection<T> Read();
    T Read(K id);
    void Update(T entity);
    void Delete(K id);
}