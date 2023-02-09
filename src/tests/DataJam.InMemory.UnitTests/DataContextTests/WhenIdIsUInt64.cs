using DataJam.InMemory.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsUInt64 : SingleEntityScenario<ulong>
{
    private const ulong EXPECTED_ID = 1234UL;

    protected override int ExpectedChangeCount => 1;
    
    protected override void ValidateId(ulong id)
    {
        id.Should().Be(EXPECTED_ID);
    }

    protected override TestEntity<ulong> BuildTestEntity()
    {
        return new TestEntity<ulong> { Id = EXPECTED_ID };
    }
}