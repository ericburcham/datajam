namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests.Domains.Family;

using Maps;

using Microsoft.EntityFrameworkCore;

using TestSupport.TestPatterns.Domains.Family;

public class MySqlFamilyMappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
