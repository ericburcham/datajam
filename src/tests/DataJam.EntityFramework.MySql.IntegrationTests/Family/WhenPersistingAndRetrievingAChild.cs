namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using TestSupport.EntityFramework;

public class WhenPersistingAndRetrievingAChild() : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild(BuildRepo())
{
    private static DomainRepository<EFFamilyDomain> BuildRepo()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new EFDomain(MySqlDependencies.Options, mappingConfigurator);
        var domainContext = new MySqlDomainContext(domain);

        return new(domainContext);
    }
}
