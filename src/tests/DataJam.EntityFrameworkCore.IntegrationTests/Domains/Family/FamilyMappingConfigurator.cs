namespace DataJam.EntityFrameworkCore.IntegrationTests.Domains.Family;

using Microsoft.EntityFrameworkCore;

using TestSupport.EntityFrameworkCore.Domains.Family;

public class FamilyMappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
