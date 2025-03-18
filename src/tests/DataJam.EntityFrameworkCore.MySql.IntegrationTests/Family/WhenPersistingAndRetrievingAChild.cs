namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests.Family;

using NUnit.Framework;

using TestSupport.EntityFrameworkCore;

[TestFixture]
public class WhenPersistingAndRetrievingAChild() : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild(BuildRepo())
{
    private static DomainRepository<EFCoreFamilyDomain> BuildRepo()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new EFCoreFamilyDomain(MySqlDependencies.Options, mappingConfigurator);
        var domainContext = new DomainContext<EFCoreFamilyDomain>(domain);

        return new(domainContext);
    }
}
