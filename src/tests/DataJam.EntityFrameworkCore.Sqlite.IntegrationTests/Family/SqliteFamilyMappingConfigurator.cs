namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;

using TestSupport.TestPatterns.Family;

public class SqliteFamilyMappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
