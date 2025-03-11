namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;

using TestSupport.TestPatterns.Family;

public class MappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
