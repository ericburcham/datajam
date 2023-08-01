namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

public abstract class InMemoryDataContextScenario
{
    private int _changeCount;

    protected abstract int ExpectedChangeCount { get; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var dataContext = new DataContext();
        _changeCount = await ArrangeAndCommitData(dataContext).ConfigureAwait(false);
    }

    [Test]
    public void TheChangeCountShouldBeCorrect()
    {
        _changeCount.Should().Be(ExpectedChangeCount);
    }

    protected abstract Task<int> ArrangeAndCommitData(IUnitOfWork dataContext);
}
