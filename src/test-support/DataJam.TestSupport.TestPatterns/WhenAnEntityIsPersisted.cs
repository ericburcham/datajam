namespace DataJam.TestSupport.TestPatterns;

using System.Threading.Tasks;

using Domains.Family;

using FluentAssertions;

using NUnit.Framework;

public abstract class WhenAnEntityIsPersisted
{
    protected WhenAnEntityIsPersisted(IRepository repository)
    {
        Repository = repository;
    }

    public IRepository Repository { get; }

    [Test]
    public void ItCanBeRetrieved()
    {
        var scalar = new GetChildByName("Kid");
        var result = Repository.Find(scalar);
        result.Name.Should().Be("Kid");
        result.Father.Name.Should().Be("Dad");
        result.Mother.Name.Should().Be("Mom");
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        Repository.Context.Add(child);
        await Repository.Context.CommitAsync();
    }
}
