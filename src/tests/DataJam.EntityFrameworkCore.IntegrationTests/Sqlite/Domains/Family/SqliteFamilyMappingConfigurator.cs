namespace DataJam.EntityFrameworkCore.IntegrationTests.Sqlite.Domains.Family;

using DataJam.TestSupport.TestPatterns.Domains.Family;

using Maps;

using Microsoft.EntityFrameworkCore;

public class SqliteFamilyMappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
