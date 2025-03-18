namespace DataJam.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing entity framework core domain repositories.</summary>
/// <param name="domains">Domains to use when constructing domain repositories.</param>
[PublicAPI]
public class EFCoreDomainRepositoryFactory(params EFCoreDomain[] domains) : DomainRepositoryFactory<EFCoreDomain>(domains)
{
    /// <summary>Initializes a new instance of the <see cref="EFCoreDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EFCoreDomainRepositoryFactory(IEnumerable<EFCoreDomain> domains)
        : this(domains.ToArray())
    {
    }

    /// <inheritdoc cref="DomainRepositoryFactory{TDomain}.BuildDomainContext{T}" />
    protected override IDomainContext<T> BuildDomainContext<T>()
    {
        var domain = Domains.OfType<T>().Single();

        return new DomainContext<T>(domain);
    }
}
