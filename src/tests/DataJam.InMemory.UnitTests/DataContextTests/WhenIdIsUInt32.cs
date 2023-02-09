using DataJam.InMemory.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.DataContextTests;

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