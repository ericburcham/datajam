namespace DataJam;

using JetBrains.Annotations;

/// <summary>Provides an implementation of the repository pattern for a domain of related entities.</summary>
/// <typeparam name="T">The type of the domain for this repository.</typeparam>
[PublicAPI]
public class DomainRepository<T> : Repository, IDomainRepository<T>
    where T : class
{
    /// <summary>Initializes a new instance of the <see cref="DomainRepository{T}" /> class.</summary>
    /// <param name="domainContext">The domain context to use.</param>
    public DomainRepository(IDomainContext<T> domainContext)
        : base(domainContext)
    {
    }

    /// <inheritdoc cref="IDomainRepository{T}.Context" />
    public new IDomainContext<T> Context => (IDomainContext<T>)base.Context;
}
