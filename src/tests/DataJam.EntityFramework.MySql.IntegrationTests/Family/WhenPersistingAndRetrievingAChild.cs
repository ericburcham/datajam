namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using System;

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
        var domain = new EFDomain(MySqlDependencies.Options, mappingConfigurator);
        var domainContext = new MySqlDomainContext(domain);

        return new(domainContext);
    }
}
