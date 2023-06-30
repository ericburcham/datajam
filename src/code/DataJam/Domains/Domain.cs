namespace DataJam;

public abstract class Domain<TConfigurationBinder, TConfigurationOptions> : IDomain<TConfigurationBinder, TConfigurationOptions>
    where TConfigurationBinder : class
{
    protected Domain(IConfigureDomainMappings<TConfigurationBinder> mappingConfigurator, TConfigurationOptions configurationOptions)
    {
        MappingConfigurator = mappingConfigurator;
        ConfigurationOptions = configurationOptions;
    }

    public TConfigurationOptions ConfigurationOptions { get; }

    public IConfigureDomainMappings<TConfigurationBinder> MappingConfigurator { get; }
}
