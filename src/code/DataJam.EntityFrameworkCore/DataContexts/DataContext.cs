namespace DataJam;

using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

/// <summary>Provides a disposable unit of work capable of both read and write operations.</summary>
public class DataContext : DbContext, IDataContext
{
    private readonly IConfigureDomainMappings<ModelBuilder>? _mappingConfigurator;

    /// <summary>Initializes a new instance of the <see cref="DataContext" /> class.</summary>
    /// <param name="configurationOptions">The configuration options.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public DataContext(DbContextOptions configurationOptions, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(configurationOptions)
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

    /// <inheritdoc cref="IDataSource.AsQueryable{TResult}" />
    public IQueryable<T> AsQueryable<T>()
        where T : class
    {
        return Set<T>();
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

        return await SaveChangesAsync();
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
