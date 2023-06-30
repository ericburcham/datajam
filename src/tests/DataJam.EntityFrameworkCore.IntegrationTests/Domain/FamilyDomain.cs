namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;

public class FamilyDomain : EntityFrameworkCoreDomain
{
    public FamilyDomain(IConfigureDomainMappings<ModelBuilder> mappingConfigurator, DbContextOptions configurationOptions)
        : base(mappingConfigurator, configurationOptions)
    {
    }
}
