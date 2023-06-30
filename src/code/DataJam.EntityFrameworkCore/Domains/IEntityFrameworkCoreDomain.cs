namespace DataJam;

using Microsoft.EntityFrameworkCore;

public interface IEntityFrameworkCoreDomain : IDomain<ModelBuilder, DbContextOptions>
{
}

public class EntityFrameworkCoreDomain : Domain<ModelBuilder, DbContextOptions>, IEntityFrameworkCoreDomain
{
    public EntityFrameworkCoreDomain(IConfigureDomainMappings<ModelBuilder> mappingConfigurator, DbContextOptions configurationOptions)
        : base(mappingConfigurator, configurationOptions)
    {
    }
}
