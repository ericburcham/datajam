namespace DataJam.EntityFrameworkCore;

using System.Linq;

using Microsoft.EntityFrameworkCore;

/// <summary>Provides a data context which is limited to read operations.</summary>
public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyDataContext
{
    private readonly IConfigureDomainMappings<ModelBuilder>? _mappingConfigurator;

    /// <summary>Initializes a new instance of the <see cref="ReadonlyDataContext" /> class.</summary>
    /// <param name="options">The configuration options.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public ReadonlyDataContext(DbContextOptions options, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(options)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <inheritdoc cref="IDataSource.CreateQuery{TResult}" />
    public IQueryable<T> CreateQuery<T>()
        where T : class
    {
        return Set<T>();
    }

    /// <inheritdoc cref="DbContext.OnModelCreating" />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _mappingConfigurator?.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
