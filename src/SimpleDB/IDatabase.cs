namespace SimpleDB;

public interface IDatabase
{
    public IEnumerable<T> Read<T>(int limit = int.MaxValue);

    public void Store<T>(T record);
}