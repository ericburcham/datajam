namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

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
            var domain = new EFFamilyDomain(MsSqlDependencies.Options, mappingConfigurator);
            var domainContext = new DomainContext<EFFamilyDomain>(domain);

            return new DomainRepository<EFFamilyDomain>(domainContext);
        }
    }
}
