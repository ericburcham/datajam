namespace DataJam.InMemory;

public class InMemoryDataContext
{
    public T Add<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }

    public int Commit()
    {
        throw new NotImplementedException();
    }
}