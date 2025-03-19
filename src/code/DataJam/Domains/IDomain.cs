namespace DataJam.Domains;

using JetBrains.Annotations;

/// <summary>Represents a domain of related entities and the configuration necessary to use them with a backing data store.</summary>
/// <typeparam name="TConfigurationBinder">The concrete type that is used to bind the configuration.</typeparam>
/// <typeparam name="TConfigurationOptions">The concrete type that is used to carry configuration options.</typeparam>
[PublicAPI]
public interface IDomain<in TConfigurationBinder, out TConfigurationOptions>
    where TConfigurationBinder : class
{
    /// <summary>Gets the configuration options.</summary>
    TConfigurationOptions ConfigurationOptions { get; }

    /// <summary>Gets the mapping configurator.</summary>
    IConfigureDomainMappings<TConfigurationBinder> MappingConfigurator { get; }
}
