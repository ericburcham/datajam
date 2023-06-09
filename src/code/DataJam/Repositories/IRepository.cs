namespace DataJam.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
///     Exposes an interface for repositories.
/// </summary>
public interface IRepository
{
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        IUnitOfWork Context { get; }

        /// <summary>
        /// Executes a specified command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        void Execute(ICommand command);

        /// <summary>
        /// Executes a specified command asynchronously.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteAsync(ICommand command);

        /// <summary>
        /// Finds a collection of items based on a specified query.
        /// </summary>
        /// <typeparam name="T">The type of the items to find.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>An enumerable of items matching the specified query.</returns>
        IEnumerable<T> Find<T>(IQuery<T> query);

        /// <summary>
        /// Finds a single item based on a specified scalar query.
        /// </summary>
        /// <typeparam name="T">The type of the item to find.</typeparam>
        /// <param name="query">The scalar query to execute.</param>
        /// <returns>The item matching the specified scalar query.</returns>
        T Find<T>(IScalar<T> query);

        /// <summary>
        /// Finds a single item based on a specified scalar query asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the item to find.</typeparam>
        /// <param name="query">The scalar query to execute.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the item matching the specified scalar query.</returns>
        Task<T> FindAsync<T>(IScalar<T> query);

        /// <summary>
        /// Finds a collection of items based on a specified query asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the items to find.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of items matching the specified query.</returns>
        Task<IEnumerable<T>> FindAsync<T>(IQuery<T> query);

        /// <summary>
        /// Finds a collection of projection items based on a specified query asynchronously.
        /// </summary>
        /// <typeparam name="TSelection">The type of the items to select from.</typeparam>
        /// <typeparam name="TProjection">The type of the items to project.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of projection items matching the specified query.</returns>
        Task<IEnumerable<TProjection>> FindAsync<TSelection, TProjection>(IQuery<TSelection, TProjection> query)
                where TSelection : class;
}
