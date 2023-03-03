using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

[TestFixture]
public class WhenIdIsInt64WithIdentity : SingleEntityScenario<long>
{
    protected override void ValidateId(long id)
    {
        id.Should().Be(1);
    }

    protected override int ExpectedChangeCount => 1;
}