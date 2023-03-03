using DataJam.Testing.UnitTests.Domains.None;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

[TestFixture]
public class WhenIdIsInt32 : SingleEntityScenario<int>
{
    private const int EXPECTED_ID = 1234;

    protected override int ExpectedChangeCount => 1;
    
    protected override void ValidateId(int id)
    {
        id.Should().Be(EXPECTED_ID);
    }

    protected override TestEntity<int> BuildTestEntity()
    {
        return new TestEntity<int> { Id = EXPECTED_ID };
    }
}