namespace DataJam.EntityFrameworkCore.IntegrationTests;

using Microsoft.EntityFrameworkCore;

public class FamilyDomain : EntityFrameworkCoreDomain
{
    public FamilyDomain(DbContextOptions configurationOptions, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(configurationOptions, mappingConfigurator)
    {
    }
}
