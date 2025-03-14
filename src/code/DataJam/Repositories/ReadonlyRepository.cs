namespace DataJam;

using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Provides a readonly implementation of the Repository pattern.</summary>
[PublicAPI]
public class ReadonlyRepository : IReadonlyRepository
{
    /// <summary>Initializes a new instance of the <see cref="ReadonlyRepository" /> class.</summary>
    /// <param name="domainContext">The data context to use.</param>
    public ReadonlyRepository(IReadonlyDataContext domainContext)
    {
        Context = domainContext;
    }

    /// <inheritdoc cref="IReadonlyRepository.Context" />
    public IReadonlyDataContext Context { get; }

    /// <inheritdoc cref="IReadonlyRepository.Find{T}(IQuery{T})" />
    public IEnumerable<T> Find<T>(IQuery<T> query)
    {
        return query.Execute(Context);
    }

    /// <inheritdoc cref="IReadonlyRepository.Find{T}(IScalar{T})" />
    public T Find<T>(IScalar<T> scalar)
    {
        return scalar.Execute(Context);
    }

    /// <inheritdoc cref="IReadonlyRepository.FindAsync{T}(IQuery{T})" />
    public Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query)
    {
        return Task.Factory.StartNew(() => query.Execute(Context));
    }

    /// <inheritdoc cref="IReadonlyRepository.FindAsync{T}(IScalar{T})" />
    public Task<T> FindAsync<T>(IScalar<T> scalar)
    {
        return Task.Factory.StartNew(() => scalar.Execute(Context));
    }
}
