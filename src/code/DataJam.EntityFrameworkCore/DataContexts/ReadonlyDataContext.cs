namespace DataJam.EntityFrameworkCore.DataContexts;

using System.Linq;

using DbContexts;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

/// <summary>Provides a data context which is limited to read operations.</summary>
/// <param name="options">The configuration options.</param>
/// <param name="mappingConfigurator">The mapping configurator to use.</param>
[PublicAPI]
public class ReadonlyDataContext(DbContextOptions options, IConfigureDomainMappings<ModelBuilder> mappingConfigurator) : ReadonlyDbContext(options), IReadonlyDataContext
{
    private readonly IConfigureDomainMappings<ModelBuilder>? _mappingConfigurator = mappingConfigurator;

    /// <inheritdoc cref="IDataSource.CreateQuery{T}" />
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
