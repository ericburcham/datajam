namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using Domains.None;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsInt16 : SingleEntityScenario<short>
{
    private const short EXPECTED_ID = 1234;

    protected override int ExpectedChangeCount => 1;

    protected override TestEntity<short> BuildTestEntity()
    {
        return new() { Id = EXPECTED_ID };
    }

    protected override void ValidateId(short id)
    {
        id.Should().Be(EXPECTED_ID);
    }
}
