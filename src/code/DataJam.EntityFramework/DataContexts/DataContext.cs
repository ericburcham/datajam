namespace DataJam.EntityFramework;

using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>Provides a combination of the Unit Of Work and Repository patterns capable of both read and write operations.</summary>
[PublicAPI]
public class DataContext : DbContext, IDataContext
{
    private readonly IConfigureDomainMappings<DbModelBuilder>? _mappingConfigurator;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class using the given string as the name or connection string for the database to which a
    ///     connection will be made. See the class remarks for how this is used to create a connection.
    /// </summary>
    /// <param name="nameOrConnectionStringProvider">Provides either the database name or a connection string.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public DataContext(IProvideNameOrConnectionString nameOrConnectionStringProvider, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(nameOrConnectionStringProvider.NameOrConnectionString)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class using the given string as the name or connection string for the database to which a
    ///     connection will be made, and initializes it from the given model. See the class remarks for how this is used to create a connection.
    /// </summary>
    /// <param name="nameOrConnectionStringProvider">Provides either the database name or a connection string.</param>
    /// <param name="model"> The model that will back this context.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public DataContext(IProvideNameOrConnectionString nameOrConnectionStringProvider, DbCompiledModel model, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(nameOrConnectionStringProvider.NameOrConnectionString, model)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class using the existing connection to connect to a database. The connection will not be
    ///     disposed when the context is disposed if <paramref name="contextOwnsConnection" /> is <c>false</c>.
    /// </summary>
    /// <param name="existingConnection">An existing connection to use for the new context.</param>
    /// <param name="contextOwnsConnection">
    ///     If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the
    ///     connection.
    /// </param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public DataContext(DbConnection existingConnection, bool contextOwnsConnection, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(existingConnection, contextOwnsConnection)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class using the existing connection to connect to a database, and initializes it from the
    ///     given model. The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" /> is <c>false</c>.
    /// </summary>
    /// <param name="existingConnection">An existing connection to use for the new context.</param>
    /// <param name="model">The model that will back this context.</param>
    /// <param name="contextOwnsConnection">
    ///     If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the
    ///     connection.
    /// </param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public DataContext(
        DbConnection existingConnection,
        DbCompiledModel model,
        bool contextOwnsConnection,
        IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(existingConnection, model, contextOwnsConnection)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>Initializes a new instance of the <see cref="DataContext" /> class around an existing ObjectContext.</summary>
    /// <param name="objectContext">An existing ObjectContext to wrap with the new context. </param>
    /// <param name="dbContextOwnsObjectContext">
    ///     If set to <c>true</c> the ObjectContext is disposed when the DbContext is disposed, otherwise the caller must dispose
    ///     the connection.
    /// </param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    public DataContext(ObjectContext objectContext, bool dbContextOwnsObjectContext, IConfigureDomainMappings<DbModelBuilder>? mappingConfigurator)
        : base(objectContext, dbContextOwnsObjectContext)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <summary>Initializes a new instance of the <see cref="DataContext" /> class.</summary>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    protected DataContext(IConfigureDomainMappings<DbModelBuilder> mappingConfigurator)
    {
        _mappingConfigurator = mappingConfigurator;
    }

    /// <inheritdoc cref="IUnitOfWork.Add{T}" />
    public T Add<T>(T item)
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
    public async Task<int> CommitAsync(CancellationToken token = default)
    {
        ChangeTracker.DetectChanges();

        return await SaveChangesAsync(token).ConfigureAwait(false);
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
    public T Remove<T>(T item)
        where T : class
    {
        return Set<T>().Remove(item);
    }

    /// <inheritdoc cref="IUnitOfWork.Update{T}" />
    public T Update<T>(T item)
        where T : class
    {
        Entry(item).State = EntityState.Modified;

        return item;
    }

    /// <inheritdoc cref="DbContext.OnModelCreating" />
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        _mappingConfigurator?.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
