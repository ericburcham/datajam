namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using System;
using System.Threading.Tasks;

using Domains.None;

using NUnit.Framework;

public abstract class SingleEntityScenario<T> : InMemoryDataContextScenario
    where T : IEquatable<T>
{
    private TestEntity<T> _testEntity = null!;

    [Test]
    public void TheGeneratedIdentityShouldBeCorrect()
    {
        ValidateId(_testEntity.Id);
    }

    protected override async Task<int> ArrangeAndCommitData(IUnitOfWork dataContext)
    {
        _testEntity = BuildTestEntity();

        dataContext.Add(_testEntity);

        return await dataContext.CommitAsync();
    }

    protected virtual TestEntity<T> BuildTestEntity()
    {
        return new();
    }

    protected abstract void ValidateId(T id);
}
