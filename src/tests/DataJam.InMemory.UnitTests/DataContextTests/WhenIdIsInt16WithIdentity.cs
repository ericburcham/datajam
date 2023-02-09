using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsInt16WithIdentity : SingleEntityScenario<short>
{
    protected override void ValidateId(short id)
    {
        id.Should().Be(1);
    }

    protected override int ExpectedChangeCount => 1;

}
