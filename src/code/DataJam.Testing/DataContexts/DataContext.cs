namespace DataJam.Testing;

using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Provides a data context useful for testing.</summary>
[PublicAPI]
public class DataContext : IDataContext, IReadonlyDataContext
{
    private readonly Queue _addQueue = new();

    private readonly Queue _removeQueue = new();

    /// <summary>Initializes a new instance of the <see cref="DataContext" /> class.</summary>
    public DataContext()
    {
        Repo = new();
        RegisterIIdentifiables();
    }

    internal DataContext(RepresentationRepository repo)
    {
        Repo = repo;
        RegisterIIdentifiables();
    }

    internal RepresentationRepository Repo { get; }

    /// <inheritdoc cref="IUnitOfWork" />
    public virtual T Add<T>(T item)
        where T : class
    {
        _addQueue.Enqueue(item);

        return item;
    }

    /// <inheritdoc cref="IUnitOfWork" />
    public virtual int Commit()
    {
        var changeCount = 0;
        changeCount += ProcessCommitQueues();
        Repo.Commit();

        return changeCount;
    }

    /// <inheritdoc cref="IUnitOfWork" />
    public virtual Task<int> CommitAsync()
    {
        var task = new Task<int>(Commit);
        task.Start();

        return task;
    }

    /// <inheritdoc cref="IDataSource" />
    public virtual IQueryable<T> CreateQuery<T>()
        where T : class
    {
        return Repo.GetRepresentations<T>();
    }

    /// <inheritdoc cref="IDataContext" />
    public void Dispose()
    {
    }

    /// <summary>Registers the given <paramref name="identityStrategy" />.</summary>
    /// <typeparam name="T">The <paramref name="identityStrategy" />'s element Type.</typeparam>
    /// <param name="identityStrategy">The identity strategy to register.</param>
    public void RegisterIdentityStrategy<T>(IIdentityStrategy<T> identityStrategy)
        where T : class
    {
        if (Repo.IdentityStrategies.ContainsKey(typeof(T)))
        {
            Repo.IdentityStrategies[typeof(T)] = obj => identityStrategy.Assign((T)obj);
        }
        else
        {
            Repo.IdentityStrategies.Add(typeof(T), obj => identityStrategy.Assign((T)obj));
        }
    }

    /// <inheritdoc cref="IUnitOfWork" />
    public virtual T Reload<T>(T item)
        where T : class
    {
        return item;
    }

    /// <inheritdoc cref="IUnitOfWork" />
    public virtual T Remove<T>(T item)
        where T : class
    {
        _removeQueue.Enqueue(item);

        return item;
    }

    /// <inheritdoc cref="IUnitOfWork" />
    public virtual T Update<T>(T item)
        where T : class
    {
        return item;
    }

    /// <summary>Processes all adds, updates, and removals.</summary>
    /// <returns>The count of items which had changes.</returns>
    protected int ProcessCommitQueues()
    {
        var changeCount = 0;
        changeCount += AddAllFromQueueIntoRepository();
        changeCount += RemoveAllFromQueueFromRepository();

        return changeCount;
    }

    private int AddAllFromQueueIntoRepository()
    {
        var changeCount = 0;

        while (_addQueue.Count > 0)
        {
            Repo.Add(_addQueue.Dequeue()!);
            ++changeCount;
        }

        return changeCount;
    }

    private void RegisterIIdentifiables()
    {
        RegisterIdentityStrategy(new GuidIdentityStrategy<IIdentifiable<Guid>>(x => x.Id));
        RegisterIdentityStrategy(new Int16IdentityStrategy<IIdentifiable<short>>(x => x.Id));
        RegisterIdentityStrategy(new Int32IdentityStrategy<IIdentifiable<int>>(x => x.Id));
        RegisterIdentityStrategy(new Int64IdentityStrategy<IIdentifiable<long>>(x => x.Id));
        RegisterIdentityStrategy(new UInt16IdentityStrategy<IIdentifiable<ushort>>(x => x.Id));
        RegisterIdentityStrategy(new UInt32IdentityStrategy<IIdentifiable<uint>>(x => x.Id));
        RegisterIdentityStrategy(new UInt64IdentityStrategy<IIdentifiable<ulong>>(x => x.Id));
    }

    private int RemoveAllFromQueueFromRepository()
    {
        var changeCount = 0;

        while (_removeQueue.Count > 0)
        {
            Repo.Remove(_removeQueue.Dequeue()!);
            ++changeCount;
        }

        return changeCount;
    }
}
