using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsUInt32WithIdentity : SingleEntityScenario<uint>
{
    protected override void ValidateId(uint id)
    {
        id.Should().Be(1U);
    }

    protected override int ExpectedChangeCount => 1;
}