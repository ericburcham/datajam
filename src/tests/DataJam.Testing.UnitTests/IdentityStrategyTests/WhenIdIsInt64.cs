namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using Domains.None;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsInt64 : SingleEntityScenario<long>
{
    private const long EXPECTED_ID = 1234L;

    protected override int ExpectedChangeCount => 1;

    protected override TestEntity<long> BuildTestEntity()
    {
        return new() { Id = EXPECTED_ID };
    }

    protected override void ValidateId(long id)
    {
        id.Should().Be(EXPECTED_ID);
    }
}
