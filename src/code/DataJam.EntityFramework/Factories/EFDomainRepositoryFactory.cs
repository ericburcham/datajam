namespace DataJam.EntityFramework;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing entity framework domain repositories.</summary>
/// <param name="domains">Domains to use when constructing domain repositories.</param>
[PublicAPI]
public class EFDomainRepositoryFactory(params EFDomain[] domains) : DomainRepositoryFactory<EFDomain>(domains)
{
    /// <summary>Initializes a new instance of the <see cref="EFDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EFDomainRepositoryFactory(IEnumerable<EFDomain> domains)
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
