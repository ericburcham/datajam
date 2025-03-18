namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

using NUnit.Framework;

using TestSupport.EntityFrameworkCore;

[TestFixture]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild
{
    protected override IRepository Repository
    {
        get
        {
            var mappingConfigurator = new MappingConfigurator();
            var domain = new EFCoreFamilyDomain(MsSqlDependencies.Options, mappingConfigurator);
            var domainContext = new DomainContext<EFCoreFamilyDomain>(domain);

            return new DomainRepository<EFCoreFamilyDomain>(domainContext);
        }
    }
}
