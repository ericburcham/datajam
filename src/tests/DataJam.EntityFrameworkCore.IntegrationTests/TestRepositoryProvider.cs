namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Collections;

using Domains.Family;

using SqlServer;

public static class TestRepositoryProvider
{
    public static IEnumerable Services
    {
        get
        {
            yield return BuildSqlServerRepository("MsSql");
        }
    }

    private static TestFixtureData BuildSqlServerRepository(string testName)
    {
        var dbContextOptions = SqlServerDependencies.Instance.Options;
        var domain = new FamilyDomain(dbContextOptions, new FamilyMappingConfigurator());
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository) { TestName = testName };
    }
}
