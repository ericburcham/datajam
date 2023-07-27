namespace DataJam.EntityFrameworkCore.IntegrationTests;

using System.Threading.Tasks;

using Domains.Family;

using Microsoft.EntityFrameworkCore;

using TestSupport;
using TestSupport.EntityFrameworkCore;

public abstract class EntityFrameworkCoreScenario<TDbContextOptionProvider, TMappingConfigurator> : TransactionalScenario
    where TDbContextOptionProvider : IProvideDbContextOptions
    where TMappingConfigurator : IConfigureDomainMappings<ModelBuilder>, new()
{
    protected EntityFrameworkCoreScenario(TDbContextOptionProvider dbContextOptionProvider)
    {
        var domain = new FamilyDomain(dbContextOptionProvider.Options, new TMappingConfigurator());

        DomainContext<FamilyDomain> context = new(domain);
        Repository = new(context);
    }

    protected DomainRepository<FamilyDomain> Repository { get; }

    protected abstract Task InsertScenarioData();

    [OneTimeSetUp]
    protected async Task OneTimeSetUp()
    {
        await InsertScenarioData();
    }
}
