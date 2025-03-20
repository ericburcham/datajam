namespace DataJam.EntityFramework.DataContexts;

using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

using Configuration;

using DataJam.DataContexts;
using DataJam.Domains;

using DbContexts;

using JetBrains.Annotations;

/// <summary>Provides a data context which is limited to read operations.</summary>
[PublicAPI]
public class ReadonlyDataContext : ReadonlyDbContext, IReadonlyDataContext
{
    private readonly IConfigureDomainMappings<DbModelBuilder>? _mappingConfigurator;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class using the given string as the name or connection string for the database to
    ///     which a connection will be made. See the class remarks for how this is used to create a connection.
    /// </summary>
    /// <param name="nameOrConnectionStringProvider">Provides either the database name or a connection string.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public ReadonlyDataContext(IProvideNameOrConnectionString nameOrConnectionStringProvider, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(nameOrConnectionStringProvider.NameOrConnectionString)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class using the given string as the name or connection string for the database to
    ///     which a connection will be made, and initializes it from the given model. See the class remarks for how this is used to create a connection.
    /// </summary>
    /// <param name="nameOrConnectionStringProvider">Provides either the database name or a connection string.</param>
    /// <param name="model"> The model that will back this context.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public ReadonlyDataContext(IProvideNameOrConnectionString nameOrConnectionStringProvider, DbCompiledModel model, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(nameOrConnectionStringProvider.NameOrConnectionString, model)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class using the existing connection to connect to a database. The connection will not
    ///     be disposed when the context is disposed if <paramref name="contextOwnsConnection" /> is <c>false</c>.
    /// </summary>
    /// <param name="existingConnection">An existing connection to use for the new context.</param>
    /// <param name="contextOwnsConnection">
    ///     If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the
    ///     connection.
    /// </param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public ReadonlyDataContext(DbConnection existingConnection, bool contextOwnsConnection, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(existingConnection, contextOwnsConnection)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReadonlyDataContext" /> class using the existing connection to connect to a database, and initializes it from
    ///     the given model. The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" /> is <c>false</c>.
    /// </summary>
    /// <param name="existingConnection">An existing connection to use for the new context.</param>
    /// <param name="model">The model that will back this context.</param>
    /// <param name="contextOwnsConnection">
    ///     If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the
    ///     connection.
    /// </param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public ReadonlyDataContext(
        DbConnection existingConnection,
        DbCompiledModel model,
        bool contextOwnsConnection,
        IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(existingConnection, model, contextOwnsConnection)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>Initializes a new instance of the <see cref="ReadonlyDataContext" /> class around an existing ObjectContext.</summary>
    /// <param name="objectContext">An existing ObjectContext to wrap with the new context. </param>
    /// <param name="dbContextOwnsObjectContext">
    ///     If set to <c>true</c> the ObjectContext is disposed when the DbContext is disposed, otherwise the caller must dispose
    ///     the connection.
    /// </param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
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
