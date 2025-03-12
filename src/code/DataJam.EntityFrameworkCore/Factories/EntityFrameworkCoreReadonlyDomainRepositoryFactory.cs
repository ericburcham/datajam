namespace DataJam.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing readonly entity framework core domain repositories.</summary>
[PublicAPI]
public class EntityFrameworkCoreReadonlyDomainRepositoryFactory : ReadonlyDomainRepositoryFactory<EntityFrameworkCoreDomain>
{
    /// <summary>Initializes a new instance of the <see cref="EntityFrameworkCoreReadonlyDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EntityFrameworkCoreReadonlyDomainRepositoryFactory(IEnumerable<EntityFrameworkCoreDomain> domains)
        : this(domains.ToArray())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="EntityFrameworkCoreReadonlyDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EntityFrameworkCoreReadonlyDomainRepositoryFactory(params EntityFrameworkCoreDomain[] domains)
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
