﻿namespace DataJam.Testing;

using System.Collections;

public class TestDataContext : IDataContext, IReadonlyDataContext
{
    internal readonly RepresentationRepository Repo;

    private readonly Queue _addQueue = new();

    private readonly Queue _removeQueue = new();

    public TestDataContext()
    {
        Repo = new();
        RegisterIIdentifiables();
    }

    internal TestDataContext(RepresentationRepository repo)
    {
        Repo = repo;
        RegisterIIdentifiables();
    }

    public virtual T Add<T>(T item)
        where T : class
    {
        _addQueue.Enqueue(item);

        return item;
    }

    public virtual IQueryable<T> AsQueryable<T>()
        where T : class
    {
        return Repo.GetRepresentations<T>();
    }

    public virtual int Commit()
    {
        var changeCount = 0;
        changeCount += ProcessCommitQueues();
        Repo.Commit();

        return changeCount;
    }

    public virtual Task<int> CommitAsync()
    {
        var task = new Task<int>(Commit);
        task.Start();

        return task;
    }

    public void Dispose()
    {
    }

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

    public virtual T Reload<T>(T item)
        where T : class
    {
        return item;
    }

    public virtual T Remove<T>(T item)
        where T : class
    {
        _removeQueue.Enqueue(item);

        return item;
    }

    public virtual T Update<T>(T item)
        where T : class
    {
        return item;
    }

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
            Repo.Add(_addQueue.Dequeue());
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
            Repo.Remove(_removeQueue.Dequeue());
            ++changeCount;
        }

        return changeCount;
    }
}
