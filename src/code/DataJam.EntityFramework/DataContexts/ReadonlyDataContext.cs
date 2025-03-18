namespace DataJam.EntityFramework;

using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

/// <summary>Provides a data context which is limited to read operations.</summary>
public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyDataContext
{
    private readonly IConfigureDomainMappings<DbModelBuilder>? _mappingConfigurator;

    public ReadonlyDataContext(string nameOrConnectionString, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(nameOrConnectionString)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    public ReadonlyDataContext(string nameOrConnectionString, DbCompiledModel model, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(nameOrConnectionString, model)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    public ReadonlyDataContext(DbConnection existingConnection, bool contextOwnsConnection, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(existingConnection, contextOwnsConnection)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    public ReadonlyDataContext(
        DbConnection existingConnection,
        DbCompiledModel model,
        bool contextOwnsConnection,
        IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(existingConnection, model, contextOwnsConnection)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    public ReadonlyDataContext(ObjectContext objectContext, bool dbContextOwnsObjectContext, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(objectContext, dbContextOwnsObjectContext)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>Initializes a new instance of the <see cref="ReadonlyDataContext" /> class.</summary>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    protected ReadonlyDataContext(IConfigureDomainMappings<DbModelBuilder> mappingConfigurator)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <inheritdoc cref="IDataSource.CreateQuery{T}" />
    public IQueryable<T> CreateQuery<T>()
        where T : class
    {
        return Set<T>();
    }

    /// <inheritdoc cref="DbContext.OnModelCreating" />
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        _mappingConfigurator?.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
