namespace DataJam;

using Microsoft.EntityFrameworkCore;

/// <summary>
///     Provides a base class for domains that is specific to Entity Framework Core.
/// </summary>
public abstract class EntityFrameworkCoreDomain : Domain<ModelBuilder, DbContextOptions>, IEntityFrameworkCoreDomain
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityFrameworkCoreDomain" /> class.
    /// </summary>
    /// <param name="configurationOptions">
    ///     The configuration options to use.
    /// </param>
    /// <param name="mappingConfigurator">
    ///     The mapping configurator to use.
    /// </param>
    protected EntityFrameworkCoreDomain(DbContextOptions configurationOptions, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(configurationOptions, mappingConfigurator)
    {
    }
}
