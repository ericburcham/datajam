namespace DataJam.EntityFrameworkCore;

using JetBrains.Annotations;

/// <summary>Provides a disposable unit of work that is capable of both read and write operations and which is organized around a domain.</summary>
/// <param name="domain">The domain to use.</param>
/// <typeparam name="T">The Type of the domain for this domain context.</typeparam>
[PublicAPI]
public class DomainContext<T>(T domain) : DataContext(domain.ConfigurationOptions, domain.MappingConfigurator), IDomainContext<T>
    where T : class, IDomain;
