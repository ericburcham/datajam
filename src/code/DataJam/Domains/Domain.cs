namespace DataJam;

using JetBrains.Annotations;

/// <summary>Provides a base class for domains.</summary>
/// <typeparam name="TConfigurationBinder">The concrete type that is used to bind the configuration.</typeparam>
/// <typeparam name="TConfigurationOptions">The concrete type that is used to carry configuration options.</typeparam>
[PublicAPI]
public abstract class Domain<TConfigurationBinder, TConfigurationOptions> : IDomain<TConfigurationBinder, TConfigurationOptions>
    where TConfigurationBinder : class
{
    /// <summary>Initializes a new instance of the <see cref="Domain{TConfigurationBinder,TConfigurationOptions}" /> class.</summary>
    /// <param name="configurationOptions">The configuration options.</param>
    /// <param name="mappingConfigurator">The mapping configurator.</param>
    protected Domain(TConfigurationOptions configurationOptions, IConfigureDomainMappings<TConfigurationBinder> mappingConfigurator)
    {
        MappingConfigurator = mappingConfigurator;
        ConfigurationOptions = configurationOptions;
    }

    /// <inheritdoc cref="IDomain{TConfigurationBinder,TConfigurationOptions}.ConfigurationOptions" />
    public TConfigurationOptions ConfigurationOptions { get; }

    /// <inheritdoc cref="IDomain{TConfigurationBinder,TConfigurationOptions}.MappingConfigurator" />
    public IConfigureDomainMappings<TConfigurationBinder> MappingConfigurator { get; }
}
