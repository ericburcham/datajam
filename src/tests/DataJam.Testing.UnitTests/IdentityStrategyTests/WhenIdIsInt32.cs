namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using Domains.None;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsInt32 : SingleEntityScenario<int>
{
    private const int EXPECTED_ID = 1234;

    protected override int ExpectedChangeCount => 1;

    protected override TestEntity<int> BuildTestEntity()
    {
        return new() { Id = EXPECTED_ID };
    }

    protected override void ValidateId(int id)
    {
        id.Should().Be(EXPECTED_ID);
    }
}
