namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;

public class FamilyMappingConfiguration : IConfigureDomainMappings
{
    public void Configure(ModelBuilder modelBuilder)
    {
        new ChildMapping().Configure(modelBuilder.Entity<Child>());
        new FatherMapping().Configure(modelBuilder.Entity<Father>());
        new MotherMapping().Configure(modelBuilder.Entity<Mother>());
    }
}
