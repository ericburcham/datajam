namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using System.Data.Entity;

using global::MySql.Data.EntityFramework;

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

[DbConfigurationType(typeof(MySqlEFConfiguration))]
internal class MySqlDomainContext(EFDomain domain) : DomainContext<EFDomain>(domain);
