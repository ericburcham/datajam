namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Threading.Tasks;

using TestSupport;

[SetUpFixture]
public class RootSetUpFixture : RootSetUpFixtureBase
{
    protected override async Task StartContainers()
    {
        await Parallel.ForEachAsync(
            ContainerProvider.Instance.Containers,
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
