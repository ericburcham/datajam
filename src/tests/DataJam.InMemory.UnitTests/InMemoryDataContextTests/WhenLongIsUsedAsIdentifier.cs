using DataJam.InMemory.UnitTests.Domains.Family;
using FluentAssertions;
using NUnit.Framework;

namespace DataJam.InMemory.UnitTests.InMemoryDataContextTests;

[TestFixture]
public class WhenLongIsUsedAsIdentifierAndASingleEntityIsInserted
{
    private Person<long> _person = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var context = new InMemoryDataContext();

        _person = new Person<long>();
        context.Add(_person);
        context.Commit();
    }

    [Test]
    public void TheEntityShouldHaveAnIdentifierOfOne()
    {
        _person.Id.Should().Be(1);
    }
}