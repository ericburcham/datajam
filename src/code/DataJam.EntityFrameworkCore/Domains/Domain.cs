namespace DataJam.EntityFrameworkCore;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

/// <summary>Provides a base class for domains that is specific to Entity Framework Core.</summary>
/// <param name="configurationOptions">The configuration options to use.</param>
/// <param name="mappingConfigurator">The mapping configurator to use.</param>
[PublicAPI]
public abstract class Domain(
    DbContextOptions configurationOptions,
    IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
    : Domain<ModelBuilder, DbContextOptions>(configurationOptions, mappingConfigurator), IDomain;
