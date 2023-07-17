namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer.DataContextTests;

using System.Linq;
using System.Threading.Tasks;

using Domains.Family;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TestSupport.EntityFrameworkCore.Domains.Family;

[TestFixture]
public class WhenAnEntityIsPersisted : SqlServerScenario
{
    [Test]
    public void ItCanBeRetrieved()
    {
        var mappingConfiguration = new FamilyMappingConfigurator();
        var domain = new FamilyDomain(DbContextOptions, mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var result = domainContext.AsQueryable<Child>().Include(child => child.Father).Include(child => child.Mother).Single();
        result.Name.Should().Be("Kid");
        result.Father.Name.Should().Be("Dad");
        result.Mother.Name.Should().Be("Mom");
    }

    [OneTimeSetUp]
    protected async Task OneTimeSetUp()
    {
        // Insert some test data.
        var mappingConfiguration = new FamilyMappingConfigurator();
        var domain = new FamilyDomain(DbContextOptions, mappingConfiguration);
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
