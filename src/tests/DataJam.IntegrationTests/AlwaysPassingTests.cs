namespace DataJam.IntegrationTests;

using FluentAssertions;

[TestFixture]
public class AlwaysPassingTests
{
    private int _one;

    private int _two;

    [Test]
    public void OneShouldBeOne()
    {
        _one.Should().Be(1);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _one = 1;
    }

    [SetUp]
    public void SetUp()
    {
        _two = 2;
    }

    [Test]
    public void TwoShouldBeTwo()
    {
        _two.Should().Be(2);
    }
}
