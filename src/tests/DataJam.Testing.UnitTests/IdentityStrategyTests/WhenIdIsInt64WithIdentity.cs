namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsInt64WithIdentity : SingleEntityScenario<long>
{
    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(long id)
    {
        id.Should().Be(1);
    }
}
