namespace DataJam;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>Provides a readonly implementation of the Repository pattern.</summary>
public class ReadonlyRepository : IReadonlyRepository
{
    /// <summary>Initializes a new instance of the <see cref="ReadonlyRepository" /> class.</summary>
    /// <param name="dataContext">The data context to use.</param>
    public ReadonlyRepository(IReadonlyDataContext dataContext)
    {
        Context = dataContext;
    }

    /// <inheritdoc cref="IReadonlyRepository" />
    public IReadonlyDataContext Context { get; }

    /// <inheritdoc cref="IReadonlyRepository" />
    public IEnumerable<T> Find<T>(IQuery<T> query)
    {
        return query.Execute(Context);
    }

    /// <inheritdoc cref="IReadonlyRepository" />
    public T Find<T>(IScalar<T> scalar)
    {
        return scalar.Execute(Context);
    }

    /// <inheritdoc cref="IReadonlyRepository" />
    public Task<T> FindAsync<T>(IScalar<T> scalar)
    {
        return Task.Factory.StartNew(() => scalar.Execute(Context));
    }

    /// <inheritdoc cref="IReadonlyRepository" />
    public Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
    {
        return Task.Factory.StartNew(() => query.Execute(Context));
    }
}
