namespace DataJam.EntityFrameworkCore;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

/// <summary>Provides a combination of the Unit Of Work and Repository patterns capable of both read and write operations.</summary>
[PublicAPI]
public class DataContext : DbContext, IEFCoreDataContext
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

    /// <inheritdoc cref="IUnitOfWork.Add{T}" />
    public new T Add<T>(T item)
        where T : class
    {
        Set<T>().Add(item);

        return item;
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

    /// <inheritdoc cref="IDataSource.CreateQuery{T}" />
    public IQueryable<T> CreateQuery<T>()
        where T : class
    {
        return Set<T>();
    }

    /// <inheritdoc cref="IUnitOfWork.Reload{T}" />
    public T Reload<T>(T item)
        where T : class
    {
        Entry(item).Reload();

        return item;
    }

    /// <inheritdoc cref="IUnitOfWork.Remove{T}" />
    public new T Remove<T>(T item)
        where T : class
    {
        return Set<T>().Remove(item).Entity;
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
