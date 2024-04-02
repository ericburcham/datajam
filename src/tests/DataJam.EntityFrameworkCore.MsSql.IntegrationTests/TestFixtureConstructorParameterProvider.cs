namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Collections;

using Family;

using TestSupport.EntityFrameworkCore;

public static class TestFixtureConstructorParameterProvider
{
    public static IEnumerable Repositories
    {
        get
        {
            yield return BuildSqlServerConstructorParameters();
        }
    }

    private static TestFixtureData BuildSqlServerConstructorParameters()
    {
        var mappingConfigurator = new MsSqlFamilyMappingConfigurator();
        var domain = new FamilyDomain(MsSqlDependencies.Instance.Options, mappingConfigurator);
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository, true) { TestName = "MsSql" };
    }
}
