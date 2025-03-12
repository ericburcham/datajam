namespace DataJam;

using JetBrains.Annotations;

/// <summary>Provides a contract for Types that configure domain mappings against the backing data store.</summary>
/// <typeparam name="TConfigurationBinder">The concrete type that is used to bind the configuration.</typeparam>
[PublicAPI]
public interface IConfigureDomainMappings<in TConfigurationBinder>
    where TConfigurationBinder : class
{
    /// <summary>Configures the domain mappings using the given <paramref name="configurationBinder" />.</summary>
    /// <param name="configurationBinder">The configuration binder to use when configuring domain mappings.</param>
    void Configure(TConfigurationBinder configurationBinder);
}
