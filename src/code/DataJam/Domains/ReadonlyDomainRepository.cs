namespace DataJam.Domains;

using JetBrains.Annotations;

/// <summary>Provides a readonly implementation of the repository pattern for a domain of related entities.</summary>
/// <param name="domainContext">The domain context to use.</param>
/// <typeparam name="T">The type of the domain for this repository.</typeparam>
[PublicAPI]
public class ReadonlyDomainRepository<T>(IReadonlyDomainContext<T> domainContext) : ReadonlyRepository(domainContext), IReadonlyDomainRepository<T>
    where T : class
{
    /// <inheritdoc cref="IReadonlyDomainRepository{T}.Context" />
    public new IReadonlyDomainContext<T> Context => (IReadonlyDomainContext<T>)base.Context;
}
