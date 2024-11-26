namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using System.Collections;

using Family;

using TestSupport.EntityFrameworkCore;

public static class TestFixtureConstructorParameterProvider
{
    public static IEnumerable Repositories
    {
        get
        {
            yield return BuildSqliteConstructorParameters();
        }
    }

    private static TestFixtureData BuildSqliteConstructorParameters()
    {
        var mappingConfigurator = new MappingConfigurator();
        var dbContextOptions = SqliteDependencies.Instance.Options;

        var domain = new FamilyDomain(
            SqliteDependencies.Instance.Options,
            mappingConfigurator,
            dbContextOptions.SupportsLocalTransactions,
            dbContextOptions.SupportsTransactionScopes);

        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository) { TestName = "Sqlite" };
    }
}
