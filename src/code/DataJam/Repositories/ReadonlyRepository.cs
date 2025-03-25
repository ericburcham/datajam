namespace DataJam;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Provides a readonly implementation of the Repository pattern.</summary>
/// <param name="domainContext">The data context to use.</param>
[PublicAPI]
public class ReadonlyRepository(IReadonlyDataContext domainContext) : IReadonlyRepository
{
    /// <inheritdoc cref="IReadonlyRepository.Context" />
    public IReadonlyDataContext Context { get; } = domainContext;

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

    /// <inheritdoc cref="IReadonlyRepository.FindAsync{T}(IQuery{T},CancellationToken)" />
    public async Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query, CancellationToken token = default)
    {
        return await Task.Run(() => query.Execute(Context), token).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IReadonlyRepository.FindAsync{T}(IScalar{T},CancellationToken)" />
    public async Task<T> FindAsync<T>(IScalar<T> scalar, CancellationToken token = default)
    {
        return await Task.Run(() => scalar.Execute(Context), token).ConfigureAwait(false);
    }
}
