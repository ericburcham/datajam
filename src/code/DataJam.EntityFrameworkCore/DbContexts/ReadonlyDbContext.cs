namespace DataJam.EntityFrameworkCore.DbContexts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

/// <summary>A ReadonlyDbContext instance represents a READONLY session with the database and can be used to query instances of your entities.</summary>
/// <remarks>
///     <para>
///         Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This includes both parallel execution of
///         async queries and any explicit concurrent use from multiple threads. Therefore, always await async calls immediately, or use separate DbContext
///         instances for operations that execute in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for
///         more information and examples.
///     </para>
///     <para>
///         Override the <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" /> method to configure the database (and other options) to be used for the
///         context. Alternatively, if you would rather perform configuration externally instead of inline in your context, you can use
///         <see cref="DbContextOptionsBuilder{TContext}" /> (or <see cref="DbContextOptionsBuilder" />) to externally create an instance of
///         <see cref="DbContextOptions{TContext}" /> (or <see cref="DbContextOptions" />) and pass it to a base constructor of <see cref="DbContext" />.
///     </para>
///     <para>
///         The model is discovered by running a set of conventions over the entity classes found in the <see cref="DbSet{TEntity}" /> properties on the derived
///         context. To further configure the model that is discovered by convention, you can override the <see cref="DbContext.OnModelCreating(ModelBuilder)" />
///         method.
///     </para>
///     <para>
///         See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>,
///         <see href="https://aka.ms/efcore-docs-query">Querying data with EF Core</see>,
///         <see href="https://aka.ms/efcore-docs-change-tracking">Changing tracking</see>, and
///         <see href="https://aka.ms/efcore-docs-saving-data">Saving data with EF Core</see> for more information and examples.
///     </para>
/// </remarks>
[PublicAPI]
public class ReadonlyDbContext : DbContext
{
    /// <inheritdoc />
    public ReadonlyDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override EntityEntry Add(object entity)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(Add)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <typeparam name="T">The entity type is not used.</typeparam>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override EntityEntry<T> Add<T>(T entity)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(Add)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <param name="cancellationToken">The cancellation token is not used.</param>
    /// <typeparam name="T">The entity type is not used.</typeparam>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override ValueTask<EntityEntry<T>> AddAsync<T>(T entity, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(AddAsync)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <param name="cancellationToken">The cancellation token is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(AddAsync)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override void AddRange(IEnumerable<object> entities)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(AddRange)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override void AddRange(params object[] entities)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(AddRange)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override Task AddRangeAsync(params object[] entities)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(AddRangeAsync)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <param name="cancellationToken">The cancellation token is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(AddRangeAsync)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override EntityEntry Remove(object entity)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(Remove)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <typeparam name="T">The entity type is not used.</typeparam>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override EntityEntry<T> Remove<T>(T entity)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(Remove)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override void RemoveRange(IEnumerable<object> entities)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(RemoveRange)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override void RemoveRange(params object[] entities)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(RemoveRange)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="acceptAllChangesOnSuccess">Not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(SaveChanges)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override int SaveChanges()
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(SaveChanges)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="cancellationToken">The cancellation token is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(SaveChangesAsync)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="acceptAllChangesOnSuccess">Not used.</param>
    /// <param name="cancellationToken">The cancellation token is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(SaveChangesAsync)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <typeparam name="TEntity">The entity type is not used.</typeparam>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override DbSet<TEntity> Set<TEntity>()
    {
        throw new InvalidOperationException($"Do not call {nameof(Set)} on a {nameof(ReadonlyDbContext)}.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="name">The name is not used.</param>
    /// <typeparam name="TEntity">The entity type is not used.</typeparam>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override DbSet<TEntity> Set<TEntity>(string name)
    {
        throw new InvalidOperationException($"Do not call {nameof(Set)} on a {nameof(ReadonlyDbContext)}.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override EntityEntry Update(object entity)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(Update)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entity">The entity is not used.</param>
    /// <typeparam name="T">The entity type is not used.</typeparam>
    /// <returns>Nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override EntityEntry<T> Update<T>(T entity)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(Update)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override void UpdateRange(params object[] entities)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(UpdateRange)} method is not supported.");
    }

    /// <summary>Should not be invoked.  Throws an <see cref="InvalidOperationException" />.</summary>
    /// <param name="entities">The entities are not used.</param>
    /// <exception cref="InvalidOperationException">Thrown on every invocation.</exception>
    public override void UpdateRange(IEnumerable<object> entities)
    {
        throw new InvalidOperationException($"{nameof(ReadonlyDbContext)} is readonly.  The {nameof(UpdateRange)} method is not supported.");
    }
}
