using DataJam.Testing.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

[TestFixture]
public class WhenIdIsUInt32 : SingleEntityScenario<uint>
{
    private const uint EXPECTED_ID = 1234U;

    protected override int ExpectedChangeCount => 1;
    
    protected override void ValidateId(uint id)
    {
        id.Should().Be(EXPECTED_ID);
    }

    protected override TestEntity<uint> BuildTestEntity()
    {
        return new TestEntity<uint> { Id = EXPECTED_ID };
    }
}