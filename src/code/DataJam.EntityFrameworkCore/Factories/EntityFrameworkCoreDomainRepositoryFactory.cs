namespace DataJam.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing entity framework core domain repositories.</summary>
[PublicAPI]
public class EntityFrameworkCoreDomainRepositoryFactory : DomainRepositoryFactory<EntityFrameworkCoreDomain>
{
    /// <summary>Initializes a new instance of the <see cref="EntityFrameworkCoreDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EntityFrameworkCoreDomainRepositoryFactory(IEnumerable<EntityFrameworkCoreDomain> domains)
        : this(domains.ToArray())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="EntityFrameworkCoreDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public EntityFrameworkCoreDomainRepositoryFactory(params EntityFrameworkCoreDomain[] domains)
        : base(domains)
    {
    }

    /// <inheritdoc cref="DomainRepositoryFactory{TDomain}.BuildDomainContext{T}" />
    protected override IDomainContext<T> BuildDomainContext<T>()
    {
        var domain = Domains.OfType<T>().Single();

        return new DomainContext<T>(domain);
    }
}
