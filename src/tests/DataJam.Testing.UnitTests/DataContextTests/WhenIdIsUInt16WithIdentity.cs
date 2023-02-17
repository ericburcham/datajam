using FluentAssertions;
using NUnit.Framework;

namespace DataJam.Testing.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsUInt16WithIdentity : SingleEntityScenario<ushort>
{
    protected override void ValidateId(ushort id)
    {
        id.Should().Be(1);
    }

    protected override int ExpectedChangeCount => 1;
}