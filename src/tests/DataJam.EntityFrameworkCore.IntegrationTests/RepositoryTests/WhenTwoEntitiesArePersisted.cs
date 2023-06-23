namespace DataJam.EntityFrameworkCore.IntegrationTests.RepositoryTests;

using System.Threading.Tasks;

using Domain;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TestSupport;

[TestFixture]
public class WhenTwoEntitiesArePersisted : TransactionalScenario
{
    private static readonly string ConnectionString = Dependencies.Instance.MsSql.GetConnectionString();

    [Test]
    public void TheyCanBeRetrieved()
    {
        var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options;
        var mappingConfiguration = new FamilyMappingConfiguration();
        var domain = new FamilyDomain(mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(dbContextOptions, domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);
        var query = new GetChildren();
        var results = domainRepository.Find(query);
        results.Should().HaveCount(2);
    }

    [OneTimeSetUp]
    protected async Task OneTimeSetUp()
    {
        // Insert some test data.
        var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options;
        var mappingConfiguration = new FamilyMappingConfiguration();
        var domain = new FamilyDomain(mappingConfiguration);
        var domainContext = new DomainContext<FamilyDomain>(dbContextOptions, domain);
        var domainRepository = new DomainRepository<FamilyDomain>(domainContext);

        // Add the first child.
        var father1 = new Father { Name = "Dad 1" };
        var mother1 = new Mother { Name = "Mom 1" };
        var child1 = new Child { Name = "Kid 1" };
        child1.AddParents(father1, mother1);

        // Add the second child.
        var father2 = new Father { Name = "Dad 2" };
        var mother2 = new Mother { Name = "Mom 2" };
        var child2 = new Child { Name = "Kid 2" };
        child2.AddParents(father2, mother2);

        domainRepository.Context.Add(child1);
        domainRepository.Context.Add(child2);

        // Query the test data.
        await domainRepository.Context.CommitAsync();
    }
}
