namespace DataJam;

/// <summary>Provides a disposable unit of work that is capable of both read and write operations and which is organized around a domain.</summary>
/// <typeparam name="TDomain">The Type of the domain for this domain context.</typeparam>
public class DomainContext<TDomain> : DataContext, IDomainContext<TDomain>
    where TDomain : class, IEntityFrameworkCoreDomain
{
    /// <summary>Initializes a new instance of the <see cref="DomainContext{TDomain}" /> class.</summary>
    /// <param name="domain">The domain to use.</param>
    public DomainContext(TDomain domain)
        : base(domain.ConfigurationOptions, domain.MappingConfigurator, domain.SupportsLocalTransactions, domain.SupportsTransactionScopes)
    {
    }
}
