namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Collections;

using Domains.Family;

using Microsoft.EntityFrameworkCore;

using MsSql;

using MySql;

using Sqlite;
using Sqlite.Domains.Family;

public static class TestFixtureConstructorParameterProvider
{
    public static IEnumerable Repositories
    {
        get
        {
            yield return BuildMySqlConstructorParameters();
            yield return BuildSqliteConstructorParameters();
            yield return BuildSqlServerConstructorParameters();
        }
    }

    private static TestFixtureData BuildConstructorParameters(DbContextOptions dbContextOptions, string testName, bool useAmbientTransaction)
    {
        var domain = new FamilyDomain(dbContextOptions, new FamilyMappingConfigurator());
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository, useAmbientTransaction) { TestName = testName };
    }

    private static TestFixtureData BuildMySqlConstructorParameters()
    {
        var domain = new FamilyDomain(MySqlDependencies.Instance.Options, new FamilyMappingConfigurator());
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository, true) { TestName = "MySql" };
    }

    private static TestFixtureData BuildSqliteConstructorParameters()
    {
        var domain = new FamilyDomain(SqliteDependencies.Instance.Options, new SqliteFamilyMappingConfigurator());
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository, false) { TestName = "Sqlite" };
    }

    private static TestFixtureData BuildSqlServerConstructorParameters()
    {
        var domain = new FamilyDomain(MsSqlDependencies.Instance.Options, new FamilyMappingConfigurator());
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository, true) { TestName = "MsSql" };
    }
}
