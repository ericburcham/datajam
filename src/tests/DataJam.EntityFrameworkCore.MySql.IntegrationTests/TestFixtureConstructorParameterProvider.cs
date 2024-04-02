﻿namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using System.Collections;

using Family;

using Microsoft.EntityFrameworkCore;

using TestSupport.EntityFrameworkCore;

public static class TestFixtureConstructorParameterProvider
{
    public static IEnumerable Repositories
    {
        get
        {
            yield return BuildMySqlConstructorParameters();
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
}
