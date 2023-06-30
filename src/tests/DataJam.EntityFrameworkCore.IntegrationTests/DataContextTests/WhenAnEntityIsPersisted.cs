namespace DataJam.EntityFrameworkCore.IntegrationTests.DataContextTests;

using System.Linq;
using System.Threading.Tasks;

using Domain;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TestSupport;

[TestFixture]
public class WhenAnEntityIsPersisted : TransactionalScenario
{
    private static readonly string ConnectionString = Dependencies.Instance.MsSql.GetConnectionString();

    [Test]
    public void ItCanBeRetrieved()
    {
        var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options;
        var mappingConfiguration = new FamilyMappingConfigurator();
        var domain = new FamilyDomain(dbContextOptions, mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var result = domainContext.AsQueryable<Child>().Single();
        result.Name.Should().Be("Kid");
        result.Father?.Name.Should().Be("Dad");
        result.Mother?.Name.Should().Be("Mother");
    }

    [OneTimeSetUp]
    protected async Task OneTimeSetUp()
    {
        // Insert some test data.
        var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options;
        var mappingConfiguration = new FamilyMappingConfigurator();
        var domain = new FamilyDomain(dbContextOptions, mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        domainContext.Add(child);

        // Query the test data.
        await domainContext.CommitAsync();
    }
}
