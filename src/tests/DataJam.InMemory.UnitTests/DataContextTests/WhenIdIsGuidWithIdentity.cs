using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsGuidWithIdentity : SingleEntityScenario<Guid>
{
    protected override void ValidateId(Guid id)
    {
        id.Should().NotBe(Guid.Empty);
    }

    protected override int ExpectedChangeCount => 1;
}
