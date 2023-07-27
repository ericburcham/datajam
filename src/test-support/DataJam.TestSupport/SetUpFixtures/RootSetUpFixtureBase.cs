namespace DataJam.TestSupport;

using System.Threading.Tasks;

using NUnit.Framework;

public abstract class RootSetUpFixtureBase
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

    protected abstract Task StartContainers();

    protected abstract Task StopContainers();
}
