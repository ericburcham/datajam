namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;

using TestSupport.Domains.Family;

public class FamilyMappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
