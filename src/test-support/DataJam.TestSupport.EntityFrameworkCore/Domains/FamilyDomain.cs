namespace DataJam.TestSupport.EntityFrameworkCore;

using DataJam.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class FamilyDomain : EntityFrameworkCoreDomain
{
    public FamilyDomain(
        DbContextOptions configurationOptions,
        IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(configurationOptions, mappingConfigurator)
    {
    }
}
