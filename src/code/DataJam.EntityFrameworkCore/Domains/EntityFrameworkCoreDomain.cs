namespace DataJam;

using Microsoft.EntityFrameworkCore;

/// <summary>Provides a base class for domains that is specific to Entity Framework Core.</summary>
public abstract class EntityFrameworkCoreDomain : Domain<ModelBuilder, DbContextOptions>, IEntityFrameworkCoreDomain
{
    /// <summary>Initializes a new instance of the <see cref="EntityFrameworkCoreDomain" /> class.</summary>
    /// <param name="configurationOptions">The configuration options to use.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    /// <param name="supportsLocalTransactions">Indicates whether the data context supports local transactions.</param>
    /// <param name="supportsTransactionScopes">Indicates whether the data context supports transaction scope participation.</param>
    protected EntityFrameworkCoreDomain(
        DbContextOptions configurationOptions,
        IConfigureDomainMappings<ModelBuilder> mappingConfigurator,
        bool supportsLocalTransactions,
        bool supportsTransactionScopes)
        : base(configurationOptions, mappingConfigurator)
    {
        SupportsLocalTransactions = supportsLocalTransactions;
        SupportsTransactionScopes = supportsTransactionScopes;
    }

    public bool SupportsLocalTransactions { get; }

    public bool SupportsTransactionScopes { get; }
}
