// ReSharper disable InconsistentNaming

namespace DataJam.EntityFrameworkCore;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

/// <summary>Provides a base class for domains that is specific to Entity Framework Core.</summary>
[PublicAPI]
public abstract class EFCoreDomain : Domain<ModelBuilder, DbContextOptions>, IEFCoreDomain
{
    /// <summary>Initializes a new instance of the <see cref="EFCoreDomain" /> class.</summary>
    /// <param name="configurationOptions">The configuration options to use.</param>
    /// <param name="mappingConfigurator">The mapping configurator to use.</param>
    protected EFCoreDomain(
        DbContextOptions configurationOptions,
        IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(configurationOptions, mappingConfigurator)
    {
    }
}
