namespace DataJam.TestSupport;

using System.Threading.Tasks;

using NUnit.Framework;

public abstract class RootSetUpFixtureBase
{
    [OneTimeSetUp]
    public virtual async Task OneTimeSetUp()
    {
        await StartContainers().ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public virtual async Task OneTimeTearDown()
    {
        await StopContainers().ConfigureAwait(false);
    }

    protected abstract Task StartContainers();

    protected abstract Task StopContainers();
}
