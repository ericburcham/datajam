namespace DataJam.EntityFramework;

using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

/// <summary>A ReadonlyDbContext instance represents a READONLY session with the database and can be used to query instances of your entities.</summary>
/// <remarks>
///     <para>The Entity Data Model backing the context can be specified in several ways.</para>
///     <para>
///         When using the Database First or Model First approach the Entity Data Model can be created using the Entity Designer (or manually through creation of
///         an EDMX file) and then this model can be specified using entity connection string or an
///         <see cref="T:System.Data.Entity.Core.EntityClient.EntityConnection" /> object. The connection to the database (including the name of the database) can
///         be specified in several ways.
///     </para>
///     <para>
///         If the parameterless ReadonlyDbContext constructor is called from a derived context, then the name of the derived context is used to find a connection
///         string in the app.config or web.config file.  If no connection string is found, then the name is passed to the DefaultConnectionFactory registered on
///         the <see cref="T:System.Data.Entity.Database" /> class.  The connection factory then uses the context name as the database name in a default connection
///         string.  (This default connection string points to (localdb)\MSSQLLocalDB unless a different DefaultConnectionFactory is registered.)
///     </para>
///     <para>
///         Instead of using the derived context name, the connection/database name can also be specified explicitly by passing the name to one of the
///         ReadonlyDbContext constructors that takes a string.  The name can also be passed in the form "name=myname", in which case the name must be found in the
///         config file or an exception will be thrown.
///     </para>
///     <para>
///         Note that the connection found in the app.config or web.config file can be a normal database connection string (not a special Entity Framework
///         connection string) in which case the ReadonlyDbContext will use Code First. However, if the connection found in the config file is a special Entity
///         Framework connection string, then the ReadonlyDbContext will use Database/Model First and the model specified in the connection string will be used.
///     </para>
///     <para>An existing or explicitly created DbConnection can also be used instead of the database/connection name.</para>
///     <para>
///         A <see cref="T:System.Data.Entity.DbModelBuilderVersionAttribute" /> can be applied to a class derived from ReadonlyDbContext to set the version of
///         conventions used by the context when it creates a model. If no attribute is applied then the latest version of conventions will be used.
///     </para>
/// </remarks>
[PublicAPI]
public class ReadonlyDbContext : DbContext
{
    /// <inheritdoc />
    public ReadonlyDbContext(string nameOrConnectionString)
        : base(nameOrConnectionString)
    {
    }

    /// <inheritdoc />
    public ReadonlyDbContext(string nameOrConnectionString, DbCompiledModel model)
        : base(nameOrConnectionString, model)
    {
    }

    /// <inheritdoc />
    public ReadonlyDbContext(DbConnection existingConnection, bool contextOwnsConnection)
        : base(existingConnection, contextOwnsConnection)
    {
    }

    /// <inheritdoc />
    public ReadonlyDbContext(
        DbConnection existingConnection,
        DbCompiledModel model,
        bool contextOwnsConnection)
        : base(existingConnection, model, contextOwnsConnection)
    {
    }

    /// <inheritdoc />
    public ReadonlyDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
        : base(objectContext, dbContextOwnsObjectContext)
    {
    }

    /// <inheritdoc />
    protected ReadonlyDbContext()
    {
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public sealed override int SaveChanges()
    {
        throw new InvalidOperationException($"Do not call {nameof(SaveChanges)} on a {nameof(ReadonlyDbContext)}.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public sealed override Task<int> SaveChangesAsync()
    {
        throw new InvalidOperationException($"Do not call {nameof(SaveChangesAsync)} on a {nameof(ReadonlyDbContext)}.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="cancellationToken">The cancellation token is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new InvalidOperationException($"Do not call {nameof(SaveChangesAsync)} on a {nameof(ReadonlyDbContext)}.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <typeparam name="TEntity">The entity type is not used.</typeparam>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public sealed override DbSet<TEntity> Set<TEntity>()
    {
        throw new InvalidOperationException($"Do not call {nameof(Set)} on a {nameof(ReadonlyDbContext)}.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entityType">The entity type is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public sealed override DbSet Set(Type entityType)
    {
        throw new InvalidOperationException($"Do not call {nameof(Set)} on a {nameof(ReadonlyDbContext)}.");
    }
}
