namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;

public class FamilyDomain : IDomain<ModelBuilder>
{
    public FamilyDomain(IConfigureDomainMappings<ModelBuilder> mappingConfiguration)
    {
        MappingConfigurator = mappingConfiguration;
    }

    public IConfigureDomainMappings<ModelBuilder> MappingConfigurator { get; }
}
