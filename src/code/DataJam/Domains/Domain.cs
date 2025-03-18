namespace DataJam;

using JetBrains.Annotations;

/// <summary>Provides a base class for domains.</summary>
/// <param name="configurationOptions">The configuration options.</param>
/// <param name="mappingConfigurator">The mapping configurator.</param>
/// <typeparam name="TConfigurationBinder">The concrete type that is used to bind the configuration.</typeparam>
/// <typeparam name="TConfigurationOptions">The concrete type that is used to carry configuration options.</typeparam>
[PublicAPI]
public abstract class Domain<TConfigurationBinder, TConfigurationOptions>(
    TConfigurationOptions configurationOptions,
    IConfigureDomainMappings<TConfigurationBinder> mappingConfigurator)
    : IDomain<TConfigurationBinder, TConfigurationOptions>
    where TConfigurationBinder : class
{
    /// <inheritdoc cref="IDomain{TConfigurationBinder,TConfigurationOptions}.ConfigurationOptions" />
    public TConfigurationOptions ConfigurationOptions { get; } = configurationOptions;

    /// <inheritdoc cref="IDomain{TConfigurationBinder,TConfigurationOptions}.MappingConfigurator" />
    public IConfigureDomainMappings<TConfigurationBinder> MappingConfigurator { get; } = mappingConfigurator;
}
