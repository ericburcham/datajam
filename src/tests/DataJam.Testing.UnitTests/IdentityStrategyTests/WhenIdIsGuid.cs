namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using Domains.None;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsGuid : SingleEntityScenario<Guid>
{
    private readonly Guid _expectedId = Guid.NewGuid();

    protected override int ExpectedChangeCount => 1;

    protected override TestEntity<Guid> BuildTestEntity()
    {
        return new() { Id = _expectedId };
    }

    protected override void ValidateId(Guid id)
    {
        id.Should().Be(_expectedId);
    }
}
