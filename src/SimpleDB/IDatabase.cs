namespace SimpleDB;

public interface IDatabase<T>
{
    public IEnumerable<T> Read(int count = int.MaxValue);

    public void Store(T record);

    public void DeleteAll();
}