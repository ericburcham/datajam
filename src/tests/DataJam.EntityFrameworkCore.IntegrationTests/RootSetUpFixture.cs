namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using Microsoft.Data.Sqlite;

using MsSql;

using MySql;

using Sqlite;

using TestSupport;
using TestSupport.Migrators;

[SetUpFixture]
public class RootSetUpFixture : RootSetUpFixtureBase
{
    private const string SQLITE_MIGRATION_ASSEMBLY = "DataJam.Migrations.Sqlite";

    private const string MSSQL_MIGRATION_ASSEMBLY = "DataJam.Migrations.MsSql";

    private const string MYSQL_MIGRATION_ASSEMBLY = MSSQL_MIGRATION_ASSEMBLY;

    public override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp().ConfigureAwait(false);

        await DeployMySql().ConfigureAwait(false);
        await DeployMsSql().ConfigureAwait(false);
        await DeploySqlite().ConfigureAwait(false);

        // await Task.WhenAll(DeployMsSql(), DeploySqlite(), DeployMySql()).ConfigureAwait(false);
    }

    public override async Task OneTimeTearDown()
    {
        await base.OneTimeTearDown().ConfigureAwait(false);

        SqliteConnection.ClearAllPools();
        var sqlLiteConnectionString = SqliteDependencies.Instance.Sqlite.GetConnectionString();
        var connectionStringBuilder = new SqliteConnectionStringBuilder(sqlLiteConnectionString);
        var path = connectionStringBuilder.DataSource;

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    protected override async Task StartContainers()
    {
        await Parallel.ForEachAsync(
                           ContainerProvider.Instance.Containers.Where(x => x.State == TestcontainersStates.Undefined),
                           async (container, token) =>
                           {
                               await container.StartAsync(token).ConfigureAwait(false);
                           })
                      .ConfigureAwait(false);
    }

    protected override async Task StopContainers()
    {
        await Parallel.ForEachAsync(
                           ContainerProvider.Instance.Containers,
                           async (container, token) =>
                           {
                               await container.StopAsync(token).ConfigureAwait(false);
                           })
                      .ConfigureAwait(false);
    }

    private static async Task DeployMsSql()
    {
        var connectionString = MsSqlDependencies.Instance.MsSql.GetConnectionString();
        var databaseDeployer = new MsSqlDatabaseDeployer(connectionString, MSSQL_MIGRATION_ASSEMBLY);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }

    private static async Task DeployMySql()
    {
        var connectionString = MySqlDependencies.Instance.MySql.GetConnectionString();
        var databaseDeployer = new MySqlDatabaseDeployer(connectionString, MYSQL_MIGRATION_ASSEMBLY);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }

    private static async Task DeploySqlite()
    {
        var connectionString = SqliteDependencies.Instance.Sqlite.GetConnectionString();
        var databaseDeployer = new SqliteDatabaseDeployer(connectionString, SQLITE_MIGRATION_ASSEMBLY);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
