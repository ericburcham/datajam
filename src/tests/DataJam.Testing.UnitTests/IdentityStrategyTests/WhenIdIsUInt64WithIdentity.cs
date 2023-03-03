using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

[TestFixture]
public class WhenIdIsUInt64WithIdentity : SingleEntityScenario<ulong>
{
    protected override void ValidateId(ulong id)
    {
        id.Should().Be(1UL);
    }

    protected override int ExpectedChangeCount => 1;
}