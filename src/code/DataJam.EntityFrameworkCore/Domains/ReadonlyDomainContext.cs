namespace DataJam.EntityFrameworkCore.Domains;

using DataContexts;

using JetBrains.Annotations;

/// <summary>Represents a disposable unit of work that is only capable of read operations.</summary>
/// <param name="domain">The domain to use.</param>
/// <typeparam name="T">The Type of the domain for this domain context.</typeparam>
[PublicAPI]
public class ReadonlyDomainContext<T>(T domain) : ReadonlyDataContext(domain.ConfigurationOptions, domain.MappingConfigurator), IReadonlyDomainContext<T>
    where T : class, IDomain;
