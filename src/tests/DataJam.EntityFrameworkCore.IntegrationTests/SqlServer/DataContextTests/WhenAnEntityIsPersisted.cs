namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer.DataContextTests;

using System.Linq;
using System.Threading.Tasks;

using Domains.Family;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

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
        var result = Context.AsQueryable<Child>().Include(child => child.Father).Include(child => child.Mother).Single();
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
        Context.Add(child);

        await Context.CommitAsync();
    }
}
