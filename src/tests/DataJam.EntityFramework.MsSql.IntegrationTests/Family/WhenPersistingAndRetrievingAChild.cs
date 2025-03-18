namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

using TestSupport.EntityFramework;

public class WhenPersistingAndRetrievingAChild() : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild(BuildRepo())
{
    private static DomainRepository<EFFamilyDomain> BuildRepo()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new EFFamilyDomain(MsSqlDependencies.Options, mappingConfigurator);
        var domainContext = new DomainContext<EFFamilyDomain>(domain);

        return new(domainContext);
    }
}
