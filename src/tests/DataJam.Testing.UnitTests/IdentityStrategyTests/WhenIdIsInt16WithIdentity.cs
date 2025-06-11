namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsInt16WithIdentity : SingleEntityScenario<short>
{
    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(short id)
    {
        id.Should().Be(1);
    }
}
