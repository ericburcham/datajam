namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using NUnit.Framework;

using TestSupport.EntityFramework;

[TestFixture]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild
{
    protected override IRepository Repository
    {
        get
        {
            var mappingConfigurator = new MappingConfigurator();
            var domain = new EFDomain(MySqlDependencies.Options, mappingConfigurator);
            var domainContext = new MySqlDomainContext(domain);

            return new DomainRepository<EFFamilyDomain>(domainContext);
        }
    }
}
