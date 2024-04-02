﻿namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Collections;

using Microsoft.EntityFrameworkCore;

using Mysql;
using Mysql.Domains.Family;

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
        }
    }

    private static TestFixtureData BuildConstructorParameters<TMappingConfigurator>(DbContextOptions dbContextOptions, string testName, bool useAmbientTransaction)
        where TMappingConfigurator : IConfigureDomainMappings<ModelBuilder>, new()
    {
        var mappingConfigurator = new TMappingConfigurator();
        var domain = new FamilyDomain(dbContextOptions, mappingConfigurator);
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        return new(domainRepository, useAmbientTransaction) { TestName = testName };
    }

    private static TestFixtureData BuildMySqlConstructorParameters()
    {
        return BuildConstructorParameters<MySqlFamilyMappingConfigurator>(MySqlDependencies.Instance.Options, "MySql", true);
    }

    private static TestFixtureData BuildSqliteConstructorParameters()
    {
        return BuildConstructorParameters<SqliteFamilyMappingConfigurator>(SqliteDependencies.Instance.Options, "Sqlite", false);
    }
}
