namespace DataJam.TestSupport.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

/// <summary>Extends <see cref="TransactionalDbContextOptions" /> with an explicit constructor that requires the caller to specify transactional support options.</summary>
public class ExplicitTransactionalDbContextOptions : TransactionalDbContextOptions
{
    /// <summary>Initializes a new instance of the <see cref="ExplicitTransactionalDbContextOptions" /> class.</summary>
    /// <param name="dbContextOptions">The inner <see cref="DbContextOptions" /> instance.</param>
    /// <param name="supportsLocalTransactions">Indicates whether the configured data provider supports local transactions.</param>
    /// <param name="supportsDistributedTransactions">Indicates whether the configured data provider supports distributed transactions.</param>
    public ExplicitTransactionalDbContextOptions(DbContextOptions dbContextOptions, bool supportsLocalTransactions = false, bool supportsDistributedTransactions = false)
        : base(dbContextOptions)
    {
        SupportsLocalTransactions = supportsLocalTransactions;
        SupportsTransactionScopes = supportsDistributedTransactions;
    }

    /// <inheritdoc cref="TransactionalDbContextOptions.SupportsLocalTransactions" />
    public override bool SupportsLocalTransactions { get; }

    /// <inheritdoc cref="TransactionalDbContextOptions.SupportsTransactionScopes" />
    public override bool SupportsTransactionScopes { get; }
}
