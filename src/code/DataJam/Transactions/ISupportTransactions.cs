namespace DataJam;

using System;
using System.Threading;
using System.Threading.Tasks;

public interface ISupportTransactions<TTransaction>
{
    /// <summary>Gets the current <typeparamref name="TTransaction" /> being used by the context, or null if no transaction is in use.</summary>
    TTransaction? CurrentTransaction { get; }

    /// <summary>Starts a new transaction.</summary>
    /// <returns>A <typeparamref name="TTransaction" /> that represents the started transaction.</returns>
    TTransaction BeginTransaction();

    /// <summary>Asynchronously starts a new transaction.</summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous transaction initialization. The task result contains a <typeparamref name="TTransaction" /> that represents the
    ///     started transaction.
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<TTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>Applies the outstanding operations in the current transaction to the database.</summary>
    void CommitTransaction();

    /// <summary>Applies the outstanding operations in the current transaction to the database.</summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>Discards the outstanding operations in the current transaction.</summary>
    void RollbackTransaction();

    /// <summary>Discards the outstanding operations in the current transaction.</summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
