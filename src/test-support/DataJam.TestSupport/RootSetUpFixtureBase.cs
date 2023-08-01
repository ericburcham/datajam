namespace DataJam.TestSupport;

using System.Threading.Tasks;

using NUnit.Framework;

public abstract class RootSetUpFixtureBase
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await StartContainers().ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await StopContainers().ConfigureAwait(false);
    }

    protected abstract Task StartContainers();

    protected abstract Task StopContainers();
}
