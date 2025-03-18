namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using System.Data.Entity;

using global::MySql.Data.EntityFramework;

public class WhenPersistingAndRetrievingAChild() : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild(BuildRepo())
{
    private static DomainRepository<EFDbConnectionDomain> BuildRepo()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new EFDbConnectionDomain(MySqlDependencies.Options, mappingConfigurator);
        var domainContext = new FooContext(domain, true);

        return new(domainContext);
    }
}

[DbConfigurationType(typeof(MySqlEFConfiguration))]
internal class FooContext(EFDbConnectionDomain domain, bool contextOwnsConnection) : DbConnectionDomainContext<EFDbConnectionDomain>(domain, contextOwnsConnection);
