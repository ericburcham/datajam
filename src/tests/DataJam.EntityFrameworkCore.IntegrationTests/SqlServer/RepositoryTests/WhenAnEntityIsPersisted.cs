namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer.RepositoryTests;

using System.Threading.Tasks;

using DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;
using DataJam.TestSupport;
using DataJam.TestSupport.Domains.Family;

using Domains.Family;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

[TestFixture]
public class WhenAnEntityIsPersisted : TransactionalScenario
{
    private static readonly string ConnectionString = SqlServerDependencies.Instance.MsSql.GetConnectionString();

    [Test]
    public void ItCanBeRetrieved()
    {
        var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options;
        var mappingConfiguration = new FamilyMappingConfigurator();
        var domain = new FamilyDomain(dbContextOptions, mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);
        var scalar = new GetChildByName("Kid");
        var result = domainRepository.Find(scalar);
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
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);
        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        domainRepository.Context.Add(child);

        // Query the test data.
        await domainRepository.Context.CommitAsync();
    }
}
