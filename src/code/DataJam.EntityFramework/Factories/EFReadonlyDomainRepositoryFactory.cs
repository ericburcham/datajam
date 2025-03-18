namespace DataJam.EntityFramework;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing readonly entity framework domain repositories.</summary>
/// <param name="domains">Domains to use when constructing domain repositories.</param>
[PublicAPI]
public class EFReadonlyDomainRepositoryFactory(params EFDomain[] domains) : ReadonlyDomainRepositoryFactory<EFDomain>(domains)
{
    /// <summary>Initializes a new instance of the <see cref="EFReadonlyDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EFReadonlyDomainRepositoryFactory(IEnumerable<EFDomain> domains)
        : this(domains.ToArray())
    {
    }

    /// <inheritdoc cref="ReadonlyDomainRepositoryFactory{TDomain}.BuildDomainContext{T}" />
    protected override IReadonlyDomainContext<T> BuildDomainContext<T>()
    {
        var domain = Domains.OfType<T>().Single();

        return new ReadonlyDomainContext<T>(domain);
    }
}
