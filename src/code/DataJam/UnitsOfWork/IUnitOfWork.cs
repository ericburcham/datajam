namespace DataJam;

using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Provides a contract for implementing the unit of work pattern.</summary>
[PublicAPI]
public interface IUnitOfWork
{
    /// <summary>Adds a new <typeparamref name="T" /> to the unit of work.</summary>
    /// <param name="item">The item to add.</param>
    /// <typeparam name="T">The <paramref name="item" />'s type.</typeparam>
    /// <returns>The <paramref name="item" /> that was added to the unit of work.</returns>
    T Add<T>(T item)
        where T : class;

    /// <summary>Saves all changes made in this unit of work.</summary>
    /// <returns>The count of items in the unit of work which had changes.</returns>
    int Commit();

    /// <summary>Saves all changes made in this unit of work.</summary>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written while saving changes.</returns>
    Task<int> CommitAsync();

    /// <summary>Reloads the provided instance of <typeparamref name="T" /> from the underlying data store.</summary>
    /// <typeparam name="T">The <paramref name="item" />'s type.</typeparam>
    /// <param name="item">The item to reload.</param>
    /// <returns>The reloaded item.</returns>
    T Reload<T>(T item)
        where T : class;

    /// <summary>Removes the given <paramref name="item" /> from the unit of work.</summary>
    /// <param name="item">The item to remove.</param>
    /// <typeparam name="T">The <paramref name="item" />'s type.</typeparam>
    /// <returns>The <paramref name="item" /> to be removed.</returns>
    T Remove<T>(T item)
        where T : class;

    /// <summary>Updates the given <paramref name="item" /> in the unit of work.</summary>
    /// <param name="item">The <paramref name="item" /> to update.</param>
    /// <typeparam name="T">The <paramref name="item" />'s type.</typeparam>
    /// <returns>The updated <paramref name="item" />.</returns>
    T Update<T>(T item)
        where T : class;
}
