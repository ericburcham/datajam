namespace DataJam;

using Microsoft.EntityFrameworkCore;

public abstract class EntityFrameworkCoreDomain : Domain<ModelBuilder, DbContextOptions>, IEntityFrameworkCoreDomain
{
    protected EntityFrameworkCoreDomain(IConfigureDomainMappings<ModelBuilder> mappingConfigurator, DbContextOptions configurationOptions)
        : base(mappingConfigurator, configurationOptions)
    {
    }
}
