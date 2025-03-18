namespace DataJam;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing domain repositories.</summary>
/// <param name="domains">Domains to use when constructing domain repositories.</param>
/// <typeparam name="TDomain">The concrete domain type from the derived implementation.</typeparam>
[PublicAPI]
public abstract class DomainRepositoryFactory<TDomain>(params TDomain[] domains) : IDomainRepositoryFactory<TDomain>
    where TDomain : class
{
    /// <summary>Initializes a new instance of the <see cref="DomainRepositoryFactory{TDomain}" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    protected DomainRepositoryFactory(IEnumerable<TDomain> domains)
        : this(domains.ToArray())
    {
    }

    /// <summary>Gets the domains to use when constructing domain repositories.</summary>
    protected TDomain[] Domains { get; } = domains;

    /// <inheritdoc cref="IDomainRepositoryFactory{TDomain}.Create{T}" />
    public IDomainRepository<T> Create<T>()
        where T : class, TDomain
    {
        var context = BuildDomainContext<T>();

        return new DomainRepository<T>(context);
    }

    /// <summary>Defers implementation of constructing a framework-specific domain context to the derived type.</summary>
    /// <typeparam name="T">The type of the domain for the new domain context.</typeparam>
    /// <returns>A newly created domain context for the specified domain.</returns>
    protected abstract IDomainContext<T> BuildDomainContext<T>()
        where T : class, TDomain;
}
