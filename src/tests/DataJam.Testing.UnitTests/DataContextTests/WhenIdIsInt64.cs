using DataJam.Testing.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsInt64 : SingleEntityScenario<long>
{
    private const long EXPECTED_ID = 1234L;

    protected override int ExpectedChangeCount => 1;
    
    protected override void ValidateId(long id)
    {
        id.Should().Be(EXPECTED_ID);
    }

    protected override TestEntity<long> BuildTestEntity()
    {
        return new TestEntity<long> { Id = EXPECTED_ID };
    }
}