namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Collections;

using Domains.Family;

using Microsoft.EntityFrameworkCore;

using SqlServer;

using TestSupport.EntityFrameworkCore;

public static class ServiceProvider
{
    public static IEnumerable Services
    {
        get
        {
            yield return BuildSqlServerRepository(SqlServerDependencies.Instance, "MSSQLServer");
        }
    }

    private static TestFixtureData BuildSqlServerRepository(IProvideDbContextOptions dbContextOptionProvider, string testName)
    {
        return new(dbContextOptionProvider) { TestName = testName };
    }
}
