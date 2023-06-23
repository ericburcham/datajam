namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using DataJam.Domain;

public class FamilyDomain : IDomain
{
    public FamilyDomain(IConfigureDomainMappings mappingConfiguration)
    {
        MappingConfiguration = mappingConfiguration;
    }

    public IConfigureDomainMappings MappingConfiguration { get; }
}
