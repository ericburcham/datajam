namespace DataJam.EntityFrameworkCore;

using JetBrains.Annotations;

/// <summary>Represents a disposable unit of work that is only capable of read operations.</summary>
/// <typeparam name="T">The Type of the domain for this domain context.</typeparam>
[PublicAPI]
public class ReadonlyDomainContext<T> : ReadonlyDataContext, IReadonlyDomainContext<T>
    where T : class, IEFCoreDomain
{
    /// <summary>Initializes a new instance of the <see cref="ReadonlyDomainContext{T}" /> class.</summary>
    /// <param name="domain">The domain to use.</param>
    public ReadonlyDomainContext(T domain)
        : base(domain.ConfigurationOptions, domain.MappingConfigurator)
    {
    }
}
