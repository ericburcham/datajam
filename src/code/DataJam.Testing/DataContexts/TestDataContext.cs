using System.Collections;

namespace DataJam.Testing;

public class TestDataContext : IUnitOfWork
{
    private readonly Queue _addQueue = new();

    private readonly ObjectRepository _repo;

    public TestDataContext()
    {
        _repo = new ObjectRepository();

        InitializeIdentityStrategies();
    }

    private void InitializeIdentityStrategies()
    {
        RegisterIdentityStrategy(new GuidIdentityStrategy<IIdentifiable<Guid>>(x => x.Id));
        RegisterIdentityStrategy(new Int16IdentityStrategy<IIdentifiable<short>>(x => x.Id));
        RegisterIdentityStrategy(new Int32IdentityStrategy<IIdentifiable<int>>(x => x.Id));
        RegisterIdentityStrategy(new Int64IdentityStrategy<IIdentifiable<long>>(x => x.Id));
        RegisterIdentityStrategy(new UInt16IdentityStrategy<IIdentifiable<ushort>>(x => x.Id));
        RegisterIdentityStrategy(new UInt32IdentityStrategy<IIdentifiable<uint>>(x => x.Id));
        RegisterIdentityStrategy(new UInt64IdentityStrategy<IIdentifiable<ulong>>(x => x.Id));
    }

    public void RegisterIdentityStrategy<T>(IIdentityStrategy<T> identityStrategy)
        where T : class
    {
        if (_repo.IdentityStrategies.ContainsKey(typeof(T)))
            _repo.IdentityStrategies[typeof(T)] = obj => identityStrategy.Assign((T)obj);
        else
            _repo.IdentityStrategies.Add(typeof(T), obj => identityStrategy.Assign((T)obj));
    }


    public T Add<T>(T item)
        where T : class
    {
        _addQueue.Enqueue(item);

        return item;
    }

    public int Commit()
    {
        var changeCount = ProcessCommitQueues();
        _repo.Commit();
        return changeCount;
    }

    public Task<int> CommitAsync()
    {
        var task = new Task<int>(Commit);
        task.Start();

        return task;
    }

    private int ProcessCommitQueues()
    {
        return AddAllFromQueueIntoRepository();
    }

    private int AddAllFromQueueIntoRepository()
    {
        var changeCount = 0;
        while (_addQueue.Count > 0)
        {
            _repo.Add(_addQueue.Dequeue());
            ++changeCount;
        }

        return changeCount;
    }
}