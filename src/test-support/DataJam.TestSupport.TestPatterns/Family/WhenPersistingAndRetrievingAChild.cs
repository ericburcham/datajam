namespace DataJam.TestSupport.TestPatterns.Family;

using System.Linq;
using System.Threading.Tasks;

using EntityFrameworkCore;

using FluentAssertions;

using NUnit.Framework;

public abstract class WhenPersistingAndRetrievingAChild : TransactionalScenario
{
    private Child _result = null!;

    protected abstract IRepository Repository { get; }

    [Test]
    public void ItShouldHaveAValidId()
    {
        _result.Id.Should().NotBe(0);
    }

    [Test]
    public void ItShouldHaveTheCorrectFather()
    {
        _result.Father.Name.Should().Be("Dad");
    }

    [Test]
    public void ItShouldHaveTheCorrectMother()
    {
        _result.Mother.Name.Should().Be("Mom");
    }

    [Test]
    public void ItShouldHaveTheCorrectName()
    {
        _result.Name.Should().Be("Kid");
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Arrange
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        Repository.Context.Add(child);
        await Repository.Context.CommitAsync().ConfigureAwait(false);

        // Act
        var scalar = new GetChildren();
        _result = Repository.Find(scalar).Single();
    }

    [OneTimeTearDown]
    protected override void OneTimeTearDown()
    {
        base.OneTimeTearDown();
        var dataContext = Repository.Context;
        dataContext.Dispose();
    }
}
