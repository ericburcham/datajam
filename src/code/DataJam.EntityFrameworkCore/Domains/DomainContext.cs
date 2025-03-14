namespace DataJam.EntityFrameworkCore;

using JetBrains.Annotations;

/// <summary>Provides a disposable unit of work that is capable of both read and write operations and which is organized around a domain.</summary>
/// <typeparam name="T">The Type of the domain for this domain context.</typeparam>
[PublicAPI]
public class DomainContext<T> : DataContext, IDomainContext<T>
    where T : class, IEFCoreDomain
{
    /// <summary>Initializes a new instance of the <see cref="DomainContext{T}" /> class.</summary>
    /// <param name="domain">The domain to use.</param>
    public DomainContext(T domain)
        : base(domain.ConfigurationOptions, domain.MappingConfigurator)
    {
    }
}
