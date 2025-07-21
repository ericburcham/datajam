namespace DataJam.RavenDb;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>Provides a combination of the Unit Of Work and Repository patterns capable of both read and write operations.</summary>
public class DataContext : IDataContext
{
    /// <inheritdoc cref="IUnitOfWork.Add{T}" />
    public T Add<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IUnitOfWork.Commit" />
    public int Commit()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IUnitOfWork.CommitAsync" />
    public Task<int> CommitAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IDataSource.CreateQuery{T}" />
    public IQueryable<T> CreateQuery<T>()
        where T : class
    {
        throw new NotImplementedException();
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IUnitOfWork.Reload{T}" />
    public T Reload<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IUnitOfWork.Remove{T}" />
    public T Remove<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IUnitOfWork.Update{T}" />
    public T Update<T>(T item)
        where T : class
    {
        throw new NotImplementedException();
    }
}
