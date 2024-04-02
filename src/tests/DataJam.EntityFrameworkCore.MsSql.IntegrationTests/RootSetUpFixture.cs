namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using TestSupport;

[SetUpFixture]
public class RootSetUpFixture : RootSetUpFixtureBase
{
    public override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp().ConfigureAwait(false);

        await DeployMsSql().ConfigureAwait(false);
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
        var databaseDeployer = new MsSqlDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
