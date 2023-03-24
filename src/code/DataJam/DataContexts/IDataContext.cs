namespace DataJam;

/// <summary>Represents a disposable unit of work capable of both read and write operations.</summary>
public interface IDataContext : IDataSource, IDisposable, IUnitOfWork
{
}
