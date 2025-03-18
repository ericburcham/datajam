namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests.Family;

using TestSupport.EntityFrameworkCore;

[TestFixture]
public class WhenPersistingAndRetrievingAChild() : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild(BuildRepo())
{
    private static DomainRepository<FamilyDomain> BuildRepo()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new FamilyDomain(SqliteDependencies.Options, mappingConfigurator);
        var domainContext = new DomainContext<FamilyDomain>(domain);

        return new(domainContext);
    }
}
