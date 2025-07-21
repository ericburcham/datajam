namespace DataJam.RavenDb;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Raven.Client.Documents;
using Raven.Client.Documents.Session;

/// <summary>Provides an asynchronous RavenDB implementation of the Unit Of Work and Repository patterns capable of both read and write operations.</summary>
[PublicAPI]
public class AsyncDataContext : IAsyncDataContext
{
    private readonly IAsyncDocumentSession _session;

    private bool _disposed;

    /// <summary>Initializes a new instance of the <see cref="AsyncDataContext" /> class using the provided document store.</summary>
    /// <param name="documentStore">The RavenDB document store.</param>
    public AsyncDataContext(IDocumentStore documentStore)
        : this(documentStore.OpenAsyncSession())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="AsyncDataContext" /> class.</summary>
    /// <param name="documentStore">The RavenDB document store.</param>
    /// <param name="database">Specifies the session's database.</param>
    public AsyncDataContext(IDocumentStore documentStore, string database)
        : this(documentStore, new SessionOptions { Database = database })
    {
    }

    /// <summary>Initializes a new instance of the <see cref="AsyncDataContext" /> class.</summary>
    /// <param name="documentStore">The RavenDB document store.</param>
    /// <param name="sessionOptions">Specifies the session's database options.</param>
    public AsyncDataContext(IDocumentStore documentStore, SessionOptions sessionOptions)
        : this(documentStore.OpenAsyncSession(sessionOptions))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="AsyncDataContext" /> class.</summary>
    /// <param name="session">The RavenDB document session.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="session" /> is null.</exception>
    public AsyncDataContext(IAsyncDocumentSession session)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    /// <inheritdoc cref="IUnitOfWork.Add{T}" />
    public T Add<T>(T item)
        where T : class
    {
        ThrowIfDisposed();

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _session.StoreAsync(item).GetAwaiter().GetResult();

        return item;
    }

    /// <inheritdoc cref="IUnitOfWork.Commit" />
    public int Commit()
    {
        ThrowIfDisposed();

        var changedEntities = _session.Advanced.WhatChanged().Count;
        _session.SaveChangesAsync().GetAwaiter().GetResult();

        return changedEntities;
    }

    /// <inheritdoc cref="IUnitOfWork.CommitAsync" />
    public async Task<int> CommitAsync(CancellationToken token = default)
    {
        ThrowIfDisposed();

        var changedEntities = _session.Advanced.WhatChanged().Count;

        await _session.SaveChangesAsync(token).ConfigureAwait(false);

        return changedEntities;
    }

    /// <inheritdoc cref="IDataSource.CreateQuery{T}" />
    public IQueryable<T> CreateQuery<T>()
        where T : class
    {
        ThrowIfDisposed();

        return _session.Query<T>();
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="IUnitOfWork.Reload{T}" />
    public T Reload<T>(T item)
        where T : class
    {
        ThrowIfDisposed();

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        // Get the document ID from the item
        var documentId = _session.Advanced.GetDocumentId(item);

        if (string.IsNullOrEmpty(documentId))
        {
            throw new InvalidOperationException("Cannot reload an item that is not tracked by the session.");
        }

        // Evict the current instance from the session
        _session.Advanced.Evict(item);

        // Load it fresh from the database
        var reloadedItem = _session.LoadAsync<T>(documentId).GetAwaiter().GetResult();

        if (reloadedItem == null)
        {
            throw new InvalidOperationException($"Document with ID '{documentId}' was not found in the database.");
        }

        return reloadedItem;
    }

    /// <inheritdoc cref="IUnitOfWork.Remove{T}" />
    public T Remove<T>(T item)
        where T : class
    {
        ThrowIfDisposed();

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _session.Delete(item);

        return item;
    }

    /// <inheritdoc cref="IUnitOfWork.Update{T}" />
    public T Update<T>(T item)
        where T : class
    {
        ThrowIfDisposed();

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        // In RavenDB, if the item is already tracked by the session,
        // changes will be automatically detected and saved on SaveChanges()

        // If the item is not tracked, we need to store it
        var documentId = _session.Advanced.GetDocumentId(item);

        if (string.IsNullOrEmpty(documentId))
        {
            _session.StoreAsync(item);
        }
        else if (!_session.Advanced.IsLoaded(documentId))
        {
            // If the item has an ID but isn't loaded in the session, we need to tell RavenDB to track it
            _session.StoreAsync(item, documentId);
        }

        return item;
    }

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _session.Dispose();
        }

        _disposed = true;
    }

    /// <summary>Throws an <see cref="ObjectDisposedException" /> if this instance has been disposed.</summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
