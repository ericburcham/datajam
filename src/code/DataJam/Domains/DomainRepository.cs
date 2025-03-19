namespace DataJam;

using JetBrains.Annotations;

/// <summary>Provides an implementation of the repository pattern for a domain of related entities.</summary>
/// <param name="domainContext">The domain context to use.</param>
/// <typeparam name="T">The type of the domain for this repository.</typeparam>
[PublicAPI]
public class DomainRepository<T>(IDomainContext<T> domainContext) : Repository(domainContext), IDomainRepository<T>
    where T : class
{
    /// <inheritdoc cref="IDomainRepository{T}.Context" />
    public new IDomainContext<T> Context => (IDomainContext<T>)base.Context;
}
