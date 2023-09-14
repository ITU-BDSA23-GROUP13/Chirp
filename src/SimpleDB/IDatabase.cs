namespace SimpleDB;

public interface IDatabase<T>
{
    public IEnumerable<T> Read(int limit = int.MaxValue);

    public void Store(T record);
}
