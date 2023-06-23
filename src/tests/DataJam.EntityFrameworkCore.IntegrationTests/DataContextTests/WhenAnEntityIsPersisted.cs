namespace DataJam.EntityFrameworkCore.IntegrationTests.DataContextTests;

using System.Linq;
using System.Threading.Tasks;

using DataJam.Domain;

using Domain;

using FluentAssertions;

[TestFixture]
public class WhenAnEntityIsPersisted
{
    private static readonly string ConnectionString = Dependencies.Instance.MsSql.GetConnectionString();

    [Test]
    public void ItCanBeRetrieved()
    {
        var mappingConfiguration = new FamilyMappingConfiguration();
        var domain = new FamilyDomain(mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(ConnectionString, domain);
        var result = domainContext.AsQueryable<Child>().Single();
        result.Name.Should().Be("Kid");
        result.Father?.Name.Should().Be("Dad");
        result.Mother?.Name.Should().Be("Mother");
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Insert some test data.
        var mappingConfiguration = new FamilyMappingConfiguration();
        var domain = new FamilyDomain(mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(ConnectionString, domain);
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        domainContext.Add(child);

        // Query the test data.
        await domainContext.CommitAsync();
    }
}
