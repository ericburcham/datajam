namespace DataJam.EntityFrameworkCore.IntegrationTests.MsSql.Domains.Family;

using DataJam.TestSupport.TestPatterns.Domains.Family;

using Microsoft.EntityFrameworkCore;

public class MsSqlFamilyMappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
