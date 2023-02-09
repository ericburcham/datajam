using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.DataContextTests;

[TestFixture]
public class WhenIdIsInt32WithIdentity : SingleEntityScenario<int>
{
    protected override void ValidateId(int id)
    {
        id.Should().Be(1);
    }

    protected override int ExpectedChangeCount => 1;
}