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
        Context = new(domain);
        Repository = new(Context);
    }

    protected DomainContext<FamilyDomain> Context { get; }

    protected DomainRepository<FamilyDomain> Repository { get; }

    protected FamilyDomain GetDomain(TDbContextOptionProvider dbContextOptionProvider, TMappingConfigurator mappingConfigurator)
    {
        return new(dbContextOptionProvider.Options, mappingConfigurator);
    }

    protected abstract Task InsertScenarioData();

    [OneTimeSetUp]
    protected async Task OneTimeSetUp()
    {
        await InsertScenarioData();
    }
}
