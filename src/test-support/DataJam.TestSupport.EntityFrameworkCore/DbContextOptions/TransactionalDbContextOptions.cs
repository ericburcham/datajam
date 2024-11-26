namespace DataJam.TestSupport.EntityFrameworkCore;

using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Infrastructure;

using DbContextOptions = Microsoft.EntityFrameworkCore.DbContextOptions;

/// <summary>Wraps a <see cref="DbContextOptions" /> instance with additional information about whether the configured data provider supports transactions.</summary>
public abstract class TransactionalDbContextOptions : DbContextOptions, IDeclareTransactionSupport
{
    private readonly DbContextOptions _dbContextOptions;

    /// <summary>Initializes a new instance of the <see cref="TransactionalDbContextOptions" /> class.</summary>
    /// <param name="dbContextOptions">The inner <see cref="DbContextOptions" /> instance.</param>
    protected TransactionalDbContextOptions(DbContextOptions dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    /// <inheritdoc cref="DbContextOptions.ContextType" />
    public override Type ContextType => _dbContextOptions.ContextType;

    public override IEnumerable<IDbContextOptionsExtension> Extensions => _dbContextOptions.Extensions;

    public override bool IsFrozen => _dbContextOptions.IsFrozen;

    /// <inheritdoc cref="IDeclareTransactionSupport.SupportsLocalTransactions" />
    public abstract bool SupportsLocalTransactions { get; }

    /// <inheritdoc cref="IDeclareTransactionSupport.SupportsTransactionScopes" />
    public abstract bool SupportsTransactionScopes { get; }

    public override bool Equals(object? obj)
    {
        return _dbContextOptions.Equals(obj);
    }

    public override TExtension? FindExtension<TExtension>()
        where TExtension : class
    {
        return _dbContextOptions.FindExtension<TExtension>();
    }

    public override void Freeze()
    {
        _dbContextOptions.Freeze();
    }

    public override TExtension GetExtension<TExtension>()
    {
        return _dbContextOptions.GetExtension<TExtension>();
    }

    public override int GetHashCode()
    {
        return _dbContextOptions.GetHashCode();
    }

    public override string? ToString()
    {
        return _dbContextOptions.ToString();
    }

    /// <inheritdoc cref="DbContextOptions.WithExtension{TExtension}" />
    public override DbContextOptions WithExtension<TExtension>(TExtension extension)
    {
        return _dbContextOptions.WithExtension(extension);
    }

    protected override bool Equals(DbContextOptions other)
    {
        return _dbContextOptions.Equals(other);
    }
}
