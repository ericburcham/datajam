namespace DataJam;

public interface IConfigureDomainMappings<in TConfigurationBinder>
    where TConfigurationBinder : class
{
    void Configure(TConfigurationBinder provider);
}
