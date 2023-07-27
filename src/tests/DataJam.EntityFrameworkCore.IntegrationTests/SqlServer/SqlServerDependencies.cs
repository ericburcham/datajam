namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using DotNet.Testcontainers.Containers;

using Microsoft.EntityFrameworkCore;

using Testcontainers.MsSql;

using TestSupport;
using TestSupport.EntityFrameworkCore;

public class SqlServerDependencies : Singleton<SqlServerDependencies>, IProvideContainers, IProvideDbContextOptions
{
    private SqlServerDependencies()
    {
        MsSql = new MsSqlBuilder().Build();
        MsSql.StartAsync().Wait();
    }

    public IEnumerable<IContainer> Containers
    {
        get
        {
            yield return MsSql;
        }
    }

    public MsSqlContainer MsSql { get; set; }

    public DbContextOptions Options => new DbContextOptionsBuilder().UseSqlServer(MsSql.GetConnectionString()).Options;

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
