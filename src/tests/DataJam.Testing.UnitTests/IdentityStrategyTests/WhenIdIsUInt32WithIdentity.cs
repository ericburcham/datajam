namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsUInt32WithIdentity : SingleEntityScenario<uint>
{
    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(uint id)
    {
        id.Should().Be(1U);
    }
}
