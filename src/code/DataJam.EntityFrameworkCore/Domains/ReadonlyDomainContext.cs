namespace DataJam.EntityFrameworkCore;

using JetBrains.Annotations;

/// <summary>Represents a disposable unit of work that is only capable of read operations.</summary>
/// <typeparam name="TDomain">The Type of the domain for this domain context.</typeparam>
[PublicAPI]
public class ReadonlyDomainContext<TDomain> : ReadonlyDataContext, IReadonlyDomainContext<TDomain>
    where TDomain : class, IEntityFrameworkCoreDomain
{
    /// <summary>Initializes a new instance of the <see cref="ReadonlyDomainContext{TDomain}" /> class.</summary>
    /// <param name="domain">The domain to use.</param>
    public ReadonlyDomainContext(TDomain domain)
        : base(domain.ConfigurationOptions, domain.MappingConfigurator)
    {
    }
}
