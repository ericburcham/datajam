namespace DataJam.EntityFramework.Factories;

using System.Collections.Generic;
using System.Linq;

using Domains;

using JetBrains.Annotations;

/// <summary>Provides a simple factory for constructing readonly entity framework domain repositories.</summary>
/// <param name="domains">Domains to use when constructing domain repositories.</param>
[PublicAPI]
public class ReadonlyDomainRepositoryFactory(params Domain[] domains) : ReadonlyDomainRepositoryFactory<Domain>(domains)
{
    /// <summary>Initializes a new instance of the <see cref="ReadonlyDomainRepositoryFactory" /> class.</summary>
    /// <param name="domains">Domains to use when constructing domain repositories.</param>
    public ReadonlyDomainRepositoryFactory(IEnumerable<Domain> domains)
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
