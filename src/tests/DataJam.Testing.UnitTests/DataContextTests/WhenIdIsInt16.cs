using DataJam.Testing.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsInt16 : SingleEntityScenario<short>
{
    private const short EXPECTED_ID = 1234;

    protected override int ExpectedChangeCount => 1;
    
    protected override void ValidateId(short id)
    {
        id.Should().Be(EXPECTED_ID);
    }

    protected override TestEntity<short> BuildTestEntity()
    {
        return new TestEntity<short> { Id = EXPECTED_ID };
    }
}