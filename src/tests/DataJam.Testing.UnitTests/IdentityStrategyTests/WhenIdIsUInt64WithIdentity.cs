namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsUInt64WithIdentity : SingleEntityScenario<ulong>
{
    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(ulong id)
    {
        id.Should().Be(1UL);
    }
}
