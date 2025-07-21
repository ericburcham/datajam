namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using Domains.None;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsUInt64 : SingleEntityScenario<ulong>
{
    private const ulong EXPECTED_ID = 1234UL;

    protected override int ExpectedChangeCount => 1;

    protected override TestEntity<ulong> BuildTestEntity()
    {
        return new() { Id = EXPECTED_ID };
    }

    protected override void ValidateId(ulong id)
    {
        id.Should().Be(EXPECTED_ID);
    }
}
