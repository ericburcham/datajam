namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

public class FamilyDomain : IDomain
{
    public FamilyDomain(IConfigureDomainMappings mappingConfiguration)
    {
        MappingConfigurator = mappingConfiguration;
    }

    public IConfigureDomainMappings MappingConfigurator { get; }
}
