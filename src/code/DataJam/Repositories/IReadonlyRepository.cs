namespace DataJam;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Exposes a readonly interface for repositories.</summary>
[PublicAPI]
public interface IReadonlyRepository
{
    /// <summary>Gets the unit of work.</summary>
    IReadonlyDataContext Context { get; }

    /// <summary>Finds a collection of items based on a specified query.</summary>
    /// <typeparam name="T">The type of the items to find.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <returns>An enumerable of items matching the specified query.</returns>
    IEnumerable<T> Find<T>(IQuery<T> query);

    /// <summary>Finds a single item based on a specified scalar query.</summary>
    /// <typeparam name="T">The type of the item to find.</typeparam>
    /// <param name="scalar">The scalar query to execute.</param>
    /// <returns>The item matching the specified scalar query.</returns>
    T Find<T>(IScalar<T> scalar);

    /// <summary>Finds a single item based on a specified scalar query asynchronously.</summary>
    /// <typeparam name="T">The type of the item to find.</typeparam>
    /// <param name="scalar">The scalar query to execute.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the item matching the specified scalar query.</returns>
    Task<T> FindAsync<T>(IScalar<T> scalar, CancellationToken token = default);

    /// <summary>Finds a collection of items based on a specified query asynchronously.</summary>
    /// <typeparam name="T">The type of the items to find.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of items matching the specified query.</returns>
    Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query, CancellationToken token = default);
}
