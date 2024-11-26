namespace DataJam;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

/// <summary>Provides a disposable unit of work capable of both read and write operations.</summary>
public class DataContext : DbContext, IDataContext, IUnitOfWork<IDbContextTransaction>
{
    private readonly IConfigureDomainMappings<ModelBuilder>? _mappingConfigurator;

    /// <summary>Initializes a new instance of the <see cref="DataContext" /> class.</summary>
    /// <param name="options">The configuration options.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public DataContext(DbContextOptions options, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(options)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <inheritdoc cref="ISupportTransactions{TTransaction}.CurrentTransaction" />
    public IDbContextTransaction? CurrentTransaction => Database.CurrentTransaction;

    /// <inheritdoc cref="IDeclareTransactionSupport.SupportsLocalTransactions" />
    public bool SupportsLocalTransactions =>
        throw new NotImplementedException($"{typeof(DataContext).FullName} does not support {nameof(SupportsLocalTransactions)} at this time.");

    /// <inheritdoc cref="IDeclareTransactionSupport.SupportsTransactionScopes" />
    public bool SupportsTransactionScopes =>
        throw new NotImplementedException($"{typeof(DataContext).FullName} does not support {nameof(SupportsTransactionScopes)} at this time.");

    /// <inheritdoc cref="IUnitOfWork.Add{T}" />
    public new T Add<T>(T item)
        where T : class
    {
        Set<T>().Add(item);

        return item;
    }

    /// <inheritdoc cref="IDataSource.AsQueryable{TResult}" />
    public IQueryable<T> AsQueryable<T>()
        where T : class
    {
        return Set<T>();
    }

    /// <inheritdoc cref="ISupportTransactions{TTransaction}.BeginTransaction" />
    public IDbContextTransaction BeginTransaction()
    {
        return Database.BeginTransaction();
    }

    /// <inheritdoc cref="ISupportTransactions{TTransaction}.BeginTransactionAsync" />
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    /// <inheritdoc cref="IUnitOfWork.Commit" />
    public int Commit()
    {
        ChangeTracker.DetectChanges();

        return SaveChanges();
    }

    /// <inheritdoc cref="IUnitOfWork.CommitAsync" />
    public async Task<int> CommitAsync()
    {
        ChangeTracker.DetectChanges();

        return await SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc cref="ISupportTransactions{TTransaction}.CommitTransaction" />
    public void CommitTransaction()
    {
        Database.CommitTransaction();
    }

    /// <inheritdoc cref="ISupportTransactions{TTransaction}.CommitTransactionAsync" />
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.CommitTransactionAsync(cancellationToken);
    }

    /// <inheritdoc cref="IUnitOfWork.Remove{T}" />
    public new T Remove<T>(T item)
        where T : class
    {
        return Set<T>().Remove(item).Entity;
    }

    /// <inheritdoc cref="ISupportTransactions{TTransaction}.RollbackTransaction" />
    public void RollbackTransaction()
    {
        Database.RollbackTransaction();
    }

    /// <inheritdoc cref="ISupportTransactions{TTransaction}.RollbackTransactionAsync" />
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.RollbackTransactionAsync(cancellationToken);
    }

    /// <inheritdoc cref="IUnitOfWork.Update{T}" />
    public new T Update<T>(T item)
        where T : class
    {
        Entry(item).State = EntityState.Modified;

        return item;
    }

    /// <inheritdoc cref="DbContext.OnModelCreating" />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _mappingConfigurator?.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
