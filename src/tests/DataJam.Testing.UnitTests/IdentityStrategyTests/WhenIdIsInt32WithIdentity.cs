namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using AwesomeAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsInt32WithIdentity : SingleEntityScenario<int>
{
    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(int id)
    {
        id.Should().Be(1);
    }
}
