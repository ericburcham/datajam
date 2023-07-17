namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using DotNet.Testcontainers.Containers;

using Testcontainers.MsSql;

using TestSupport;

public class SqlServerDependencies : Singleton<SqlServerDependencies>, IDeployDatabases, IProvideContainers
{
    private SqlServerDependencies()
    {
        MsSql = new MsSqlBuilder().Build();
    }

    public IEnumerable<IContainer> Containers
    {
        get
        {
            yield return MsSql;
        }
    }

    public MsSqlContainer MsSql { get; set; }

    private static Assembly MigrationAssembly => Assembly.Load("DataJam.Migrations");

    public Task DeployDatabase()
    {
        var connectionString = Instance.MsSql.GetConnectionString();

        EnsureDatabase.For.SqlDatabase(connectionString);
        var upgradeResult = DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(MigrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
