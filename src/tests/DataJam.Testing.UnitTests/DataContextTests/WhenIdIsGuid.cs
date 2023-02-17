using DataJam.Testing.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsGuid : SingleEntityScenario<Guid>
{
    private readonly Guid _expectedId = Guid.NewGuid();

    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(Guid id)
    {
        id.Should().Be(_expectedId);
    }

    protected override TestEntity<Guid> BuildTestEntity()
    {
        return new TestEntity<Guid> { Id = _expectedId };
    }
}