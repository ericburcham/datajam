using DataJam.Testing.UnitTests.Domains.None;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.DataContextTests;

public abstract class SingleEntityScenario<T> : InMemoryDataContextScenario where T : IEquatable<T>
{
    private TestEntity<T> _testEntity = null!;

    [Test]
    public void TheGeneratedIdentityShouldBeCorrect()
    {
        ValidateId(_testEntity.Id);
    }

    protected abstract void ValidateId(T id);

    protected override async Task<int> ArrangeAndCommitData(IUnitOfWork dataContext)
    {
        _testEntity = BuildTestEntity();
        
        dataContext.Add(_testEntity);

        return await dataContext.CommitAsync();
    }

    protected virtual TestEntity<T> BuildTestEntity()
    {
        return new TestEntity<T>();
    }
}