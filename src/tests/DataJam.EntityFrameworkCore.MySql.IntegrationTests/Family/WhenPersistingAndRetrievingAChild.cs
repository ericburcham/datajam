namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests.Family;

using TestSupport.EntityFrameworkCore;

public class WhenPersistingAndRetrievingAChild() : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild(BuildRepo())
{
    private static IRepository BuildRepo()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new FamilyDomain(MySqlDependencies.Instance.Options, mappingConfigurator);
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return domainRepository;
    }
}
