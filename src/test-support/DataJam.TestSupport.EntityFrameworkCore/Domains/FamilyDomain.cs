namespace DataJam.TestSupport.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class FamilyDomain : EntityFrameworkCoreDomain
{
    public FamilyDomain(
        DbContextOptions configurationOptions,
        IConfigureDomainMappings<ModelBuilder> mappingConfigurator,
        bool supportsLocalTransactions,
        bool supportsTransactionScopes)
        : base(configurationOptions, mappingConfigurator, supportsLocalTransactions, supportsTransactionScopes)
    {
    }
}
