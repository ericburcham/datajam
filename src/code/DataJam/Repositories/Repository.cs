namespace DataJam;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>Provides an implementation of the Repository pattern.</summary>
public class Repository : IRepository
{
    /// <summary>Initializes a new instance of the <see cref="Repository" /> class.</summary>
    /// <param name="dataContext">The data context to use.</param>
    public Repository(IDataContext dataContext)
    {
        Context = dataContext;
    }

    /// <inheritdoc cref="IRepository" />
    public IDataContext Context { get; }

    /// <inheritdoc cref="IRepository" />
    public void Execute(ICommand command)
    {
        command.Execute(Context);
    }

    /// <inheritdoc cref="IRepository" />
    public Task ExecuteAsync(ICommand command)
    {
        return Task.Factory.StartNew(() => command.Execute(Context));
    }

    /// <inheritdoc cref="IRepository" />
    public IEnumerable<T> Find<T>(IQuery<T> query)
    {
        return query.Execute(Context);
    }

    /// <inheritdoc cref="IRepository" />
    public T Find<T>(IScalar<T> scalar)
    {
        return scalar.Execute(Context);
    }

    /// <inheritdoc cref="IRepository" />
    public Task<T> FindAsync<T>(IScalar<T> scalar)
    {
        return Task.Factory.StartNew(() => scalar.Execute(Context));
    }

    /// <inheritdoc cref="IRepository" />
    public Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
    {
        return Task.Factory.StartNew(() => query.Execute(Context));
    }
}
