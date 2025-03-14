// ReSharper disable InconsistentNaming

namespace DataJam.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing readonly entity framework core domain repositories.</summary>
[PublicAPI]
public class EFCoreReadonlyDomainRepositoryFactory : ReadonlyDomainRepositoryFactory<EFCoreDomain>
{
    /// <summary>Initializes a new instance of the <see cref="EFCoreReadonlyDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EFCoreReadonlyDomainRepositoryFactory(IEnumerable<EFCoreDomain> domains)
        : this(domains.ToArray())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="EFCoreReadonlyDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EFCoreReadonlyDomainRepositoryFactory(params EFCoreDomain[] domains)
        : base(domains)
    {
    }

    /// <inheritdoc cref="ReadonlyDomainRepositoryFactory{TDomain}.BuildDomainContext{T}" />
    protected override IReadonlyDomainContext<T> BuildDomainContext<T>()
    {
        var domain = Domains.OfType<T>().Single();

        return new ReadonlyDomainContext<T>(domain);
    }
}
