namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

using System;

using Domains;

using NUnit.Framework;

using TestSupport.EntityFramework;

[TestFixture]
public class WhenPersistingAndRetrievingAChild : TestSupport.TestPatterns.Family.WhenPersistingAndRetrievingAChild
{
    private readonly Lazy<IRepository> _repository = new(ValueFactory);

    protected override IRepository Repository => _repository.Value;

    private static DomainRepository<EFFamilyDomain> ValueFactory()
    {
        var mappingConfigurator = new MappingConfigurator();
        var domain = new EFFamilyDomain(MsSqlDependencies.Options, mappingConfigurator);
        var domainContext = new DomainContext<EFFamilyDomain>(domain);

        return new(domainContext);
    }
}
