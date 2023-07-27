namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Threading.Tasks;

using TestSupport;

[SetUpFixture]
public class RootSetUpFixture : RootSetUpFixtureBase
{
    protected override Task StartContainers()
    {
        // Nothing to do here.
        return Task.CompletedTask;
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
