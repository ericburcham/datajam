namespace DataJam;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>Provides an implementation of the Repository pattern.</summary>
public class Repository : IRepository
{
    private readonly IDataContext _dataContext;

    /// <summary>Initializes a new instance of the <see cref="Repository" /> class.</summary>
    /// <param name="dataContext">The data context to use.</param>
    public Repository(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <inheritdoc cref="IRepository" />
    public IUnitOfWork Context => _dataContext;

    /// <inheritdoc cref="IRepository" />
    public void Execute(ICommand command)
    {
        command.Execute(_dataContext);
    }

    /// <inheritdoc cref="IRepository" />
    public Task ExecuteAsync(ICommand command)
    {
        return Task.Factory.StartNew(() => command.Execute(_dataContext));
    }

    /// <inheritdoc cref="IRepository" />
    public IEnumerable<T> Find<T>(IQuery<T> query)
    {
        return query.Execute(_dataContext);
    }

    /// <inheritdoc cref="IRepository" />
    public T Find<T>(IScalar<T> scalar)
    {
        return scalar.Execute(_dataContext);
    }

    /// <inheritdoc cref="IRepository" />
    public IEnumerable<TProjection> Find<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
        where TSelection : class
    {
        return query.Execute(_dataContext);
    }

    /// <inheritdoc cref="IRepository" />
    public Task<T> FindAsync<T>(IScalar<T> scalar)
    {
        return Task.Factory.StartNew(() => scalar.Execute(_dataContext));
    }

    /// <inheritdoc cref="IRepository" />
    public Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
    {
        return Task.Factory.StartNew(() => query.Execute(_dataContext));
    }

    /// <inheritdoc cref="IRepository" />
    public Task<IEnumerable<TProjection>> FindAsync<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
        where TSelection : class
    {
        return Task.Factory.StartNew(() => query.Execute(_dataContext));
    }
}
