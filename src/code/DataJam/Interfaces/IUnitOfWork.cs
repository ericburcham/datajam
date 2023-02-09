namespace DataJam;

public interface IUnitOfWork
{
    T Add<T>(T item) where T : class;
    
    int Commit();

    Task<int> CommitAsync();
}
