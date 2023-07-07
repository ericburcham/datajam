namespace DataJam;

/// <summary>
///     Provides an implementation of the repository pattern for a domain of related entities.
/// </summary>
/// <typeparam name="TDomain">
///     The type of the domain for this repository.
/// </typeparam>
public class DomainRepository<TDomain> : Repository, IDomainRepository<TDomain>
    where TDomain : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DomainRepository{TDomain}" /> class.
    /// </summary>
    /// <param name="domainContext">
    ///     The domain context to use.
    /// </param>
    public DomainRepository(IDomainContext<TDomain> domainContext)
        : base(domainContext)
    {
    }

    /// <inheritdoc cref="IDomainRepository{TDomain}.Context" />
    public new IDomainContext<TDomain> Context => (IDomainContext<TDomain>)base.Context;
}
