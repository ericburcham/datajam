using DataJam.Testing.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

[TestFixture]
public class WhenIdIsUInt16 : SingleEntityScenario<ushort>
{
    private const ushort EXPECTED_ID = 1234;

    protected override int ExpectedChangeCount => 1;
    
    protected override void ValidateId(ushort id)
    {
        id.Should().Be(EXPECTED_ID);
    }

    protected override TestEntity<ushort> BuildTestEntity()
    {
        return new TestEntity<ushort> { Id = EXPECTED_ID };
    }
}