namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer.RepositoryTests;

using System.Threading.Tasks;

using Domains.Family;

using FluentAssertions;

using TestSupport.EntityFrameworkCore.Domains.Family;

[TestFixture]
public class WhenTwoEntitiesArePersisted : EntityFrameworkCoreScenario<SqlServerDependencies, FamilyMappingConfigurator>
{
    public WhenTwoEntitiesArePersisted()
        : base(SqlServerDependencies.Instance)
    {
    }

    [Test]
    public void TheyCanBeRetrieved()
    {
        var query = new GetChildren();
        var results = Repository.Find(query);
        results.Should().HaveCount(2);
    }

    protected override async Task InsertScenarioData()
    {
        // Add the first child.
        var father1 = new Father { Name = "Dad 1" };
        var mother1 = new Mother { Name = "Mom 1" };
        var child1 = new Child { Name = "Kid 1" };
        child1.AddParents(father1, mother1);
        Repository.Context.Add(child1);

        // Add the second child.
        var father2 = new Father { Name = "Dad 2" };
        var mother2 = new Mother { Name = "Mom 2" };
        var child2 = new Child { Name = "Kid 2" };
        child2.AddParents(father2, mother2);
        Repository.Context.Add(child2);

        await Repository.Context.CommitAsync();
    }
}
