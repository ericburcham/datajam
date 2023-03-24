namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

using FluentAssertions;

using NUnit.Framework;

[TestFixture]
public class WhenIdIsUInt16WithIdentity : SingleEntityScenario<ushort>
{
    protected override int ExpectedChangeCount => 1;

    protected override void ValidateId(ushort id)
    {
        id.Should().Be(1);
    }
}
