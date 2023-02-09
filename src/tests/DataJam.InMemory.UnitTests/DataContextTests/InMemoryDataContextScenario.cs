using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.DataContextTests;

public abstract class InMemoryDataContextScenario
{
    private int _changeCount;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var dataContext = new InMemoryDataContext();
        _changeCount = await ArrangeAndCommitData(dataContext);
    }

    [Test]
    public void TheChangeCountShouldBeCorrect()
    {
        _changeCount.Should().Be(ExpectedChangeCount);
    }

    protected abstract int ExpectedChangeCount { get; }

    protected abstract Task<int> ArrangeAndCommitData(IUnitOfWork dataContext);
}