namespace DataJam;

public interface IDomain<in TConfigurationBinder, out TConfigurationOptions>
    where TConfigurationBinder : class
{
    TConfigurationOptions ConfigurationOptions { get; }

    IConfigureDomainMappings<TConfigurationBinder> MappingConfigurator { get; }
}
