namespace DataJam;

public interface IDomain<in TConfigurationBinder>
    where TConfigurationBinder : class
{
    IConfigureDomainMappings<TConfigurationBinder> MappingConfigurator { get; }
}
