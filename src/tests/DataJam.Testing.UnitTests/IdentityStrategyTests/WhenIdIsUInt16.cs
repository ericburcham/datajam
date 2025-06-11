namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using Domains.None;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsUInt16 : SingleEntityScenario<ushort>
{
    private const ushort EXPECTED_ID = 1234;

    protected override int ExpectedChangeCount => 1;

    protected override TestEntity<ushort> BuildTestEntity()
    {
        return new() { Id = EXPECTED_ID };
    }

    protected override void ValidateId(ushort id)
    {
        id.Should().Be(EXPECTED_ID);
    }
}
