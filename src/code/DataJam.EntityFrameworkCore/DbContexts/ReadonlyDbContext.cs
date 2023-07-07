namespace DataJam;

using Microsoft.EntityFrameworkCore;

/// <summary>A ReadonlyDbContext instance represents a session with the database and can be used to query instances of your entities.</summary>
/// <remarks>
///     <para>
///         Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This includes both parallel execution of
///         async queries and any explicit concurrent use from multiple threads. Therefore, always await async calls immediately, or use separate DbContext
///         instances for operations that execute in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for
///         more information and examples.
///     </para>
///     <para>
///         Typically you create a class that derives from ReadonlyDbContext and contains <see cref="DbSet{TEntity}" /> properties for each entity in the model. If
///         the <see cref="DbSet{TEntity}" /> properties have a public setter, they are automatically initialized when the instance of the derived context is
///         created.
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
public class ReadonlyDbContext : DbContext
{
}
