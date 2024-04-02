namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using Microsoft.Data.Sqlite;

using TestSupport;

[SetUpFixture]
public class RootSetUpFixture : RootSetUpFixtureBase
{
    public override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp().ConfigureAwait(false);

        await DeploySqlite().ConfigureAwait(false);
    }

    public override async Task OneTimeTearDown()
    {
        await base.OneTimeTearDown().ConfigureAwait(false);

        SqliteConnection.ClearAllPools();
        var sqlLiteConnectionString = SqliteDependencies.SqliteMockContainer.GetConnectionString();
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

    private static async Task DeploySqlite()
    {
        var connectionString = SqliteDependencies.SqliteMockContainer.GetConnectionString();
        var databaseDeployer = new SqliteDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
