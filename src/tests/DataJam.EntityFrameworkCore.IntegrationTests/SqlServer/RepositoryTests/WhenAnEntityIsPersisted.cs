namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer.RepositoryTests;

using System.Threading.Tasks;

using Domains.Family;

using FluentAssertions;

using TestSupport.EntityFrameworkCore.Domains.Family;

[TestFixture]
public class WhenAnEntityIsPersisted : EntityFrameworkCoreScenario<SqlServerDependencies, FamilyMappingConfigurator>
{
    public WhenAnEntityIsPersisted()
        : base(SqlServerDependencies.Instance)
    {
    }

    [Test]
    public void ItCanBeRetrieved()
    {
        var scalar = new GetChildByName("Kid");
        var result = Repository.Find(scalar);
        result.Name.Should().Be("Kid");
        result.Father.Name.Should().Be("Dad");
        result.Mother.Name.Should().Be("Mom");
    }

    protected override async Task InsertScenarioData()
    {
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        Repository.Context.Add(child);
        await Repository.Context.CommitAsync();
    }
}
