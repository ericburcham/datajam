namespace DataJam
{
    public interface IUnitOfWork
    {
        T Add<T>(T item)
            where T : class;

        int Commit();

        Task<int> CommitAsync();

        T Reload<T>(T item)
            where T : class;

        T Remove<T>(T item)
            where T : class;

        T Update<T>(T item)
            where T : class;
    }
}
