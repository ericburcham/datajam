namespace DataJam
{
    public interface IDataSource
    {
        IQueryable<T> AsQueryable<T>()
            where T : class;
    }
}