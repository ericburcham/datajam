namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

using System.Threading.Tasks;

using DataJam.EntityFrameworkCore.IntegrationTests.Domains.Family;
using DataJam.TestSupport.EntityFrameworkCore;
using DataJam.TestSupport.EntityFrameworkCore.Domains.Family;

using FluentAssertions;

[TestFixtureSource(typeof(ServiceProvider), nameof(ServiceProvider.Services))]
public class WhenAnEntityIsPersisted
{
    private readonly IProvideDbContextOptions _dbContextOptionProvider;

    public WhenAnEntityIsPersisted(IProvideDbContextOptions dbContextOptionProvider)
    {
        _dbContextOptionProvider = dbContextOptionProvider;
    }

    public IRepository Repository { get; private set; } = null!;

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
        var domain = new FamilyDomain(_dbContextOptionProvider.Options, new FamilyMappingConfigurator());
        var domainContext = new DomainContext<FamilyDomain>(domain);
        Repository = new DomainRepository<FamilyDomain>(domainContext);

        var father = new Father { Name = "Dad" };
        var mother = new Mother { Name = "Mom" };
        var child = new Child { Name = "Kid" };
        child.AddParents(father, mother);
        Repository.Context.Add(child);
        await Repository.Context.CommitAsync();
    }
}
