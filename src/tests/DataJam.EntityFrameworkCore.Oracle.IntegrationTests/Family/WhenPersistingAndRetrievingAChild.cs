namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests.Family;

using System;

using NUnit.Framework;

using TestSupport.EntityFrameworkCore;

[TestFixture]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild
{
    private readonly Lazy<IRepository> _repository = new(ValueFactory);

    protected override IRepository Repository => _repository.Value;

    private static DomainRepository<EFCoreFamilyDomain> ValueFactory()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new EFCoreFamilyDomain(OracleDependencies.Options, mappingConfigurator);
        var domainContext = new DomainContext<EFCoreFamilyDomain>(domain);

        return new(domainContext);
    }
}
