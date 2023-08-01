namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using TestSupport;

[SetUpFixture]
public class RootSetUpFixture : RootSetUpFixtureBase
{
    protected override async Task StartContainers()
    {
        await Parallel.ForEachAsync(
            ContainerProvider.Instance.Containers.Where(x => x.State == TestcontainersStates.Undefined),
            async (container, token) =>
            {
                await container.StartAsync(token);
            });
    }

    protected override async Task StopContainers()
    {
        await Parallel.ForEachAsync(
            ContainerProvider.Instance.Containers,
            async (container, token) =>
            {
                await container.StopAsync(token);
            });
    }
}
