using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.IdentityStrategyTests;

[TestFixture]
public class WhenIdIsGuidWithIdentity : SingleEntityScenario<Guid>
{
    protected override void ValidateId(Guid id)
    {
        id.Should().NotBe(Guid.Empty);
    }

    protected override int ExpectedChangeCount => 1;
}
