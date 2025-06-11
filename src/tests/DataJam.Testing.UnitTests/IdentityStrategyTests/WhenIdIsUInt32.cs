namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using Domains.None;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsUInt32 : SingleEntityScenario<uint>
{
    private const uint EXPECTED_ID = 1234U;

    protected override int ExpectedChangeCount => 1;

    protected override TestEntity<uint> BuildTestEntity()
    {
        return new() { Id = EXPECTED_ID };
    }

    protected override void ValidateId(uint id)
    {
        id.Should().Be(EXPECTED_ID);
    }
}
