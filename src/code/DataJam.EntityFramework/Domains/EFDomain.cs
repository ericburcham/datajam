namespace DataJam.EntityFramework;

using System.Data.Entity;

using JetBrains.Annotations;

/// <summary>Provides a base class for domains that is specific to Entity Framework.</summary>
/// <param name="configurationOptions">The configuration options to use.</param>
/// <param name="mappingConfigurator">The mapping configurator to use.</param>
[PublicAPI]
public class EFDomain(IProvideNameOrConnectionString configurationOptions, IConfigureDomainMappings<DbModelBuilder> mappingConfigurator)
    : Domain<DbModelBuilder, IProvideNameOrConnectionString>(configurationOptions, mappingConfigurator), IEFDomain;
