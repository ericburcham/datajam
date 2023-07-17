namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Threading.Tasks;

[SetUpFixture]
public class SetUpFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await StartContainers();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await StopContainers();
    }

    private async Task StartContainers()
    {
        await Parallel.ForEachAsync(
            ContainerProvider.Instance.Containers,
            async (container, token) =>
            {
                await container.StartAsync(token);
            });
    }

    private async Task StopContainers()
    {
        await Parallel.ForEachAsync(
            ContainerProvider.Instance.Containers,
            async (container, token) =>
            {
                await container.StopAsync(token);
            });
    }
}
