namespace DataJam;

using JetBrains.Annotations;

/// <summary>Provides a readonly implementation of the repository pattern for a domain of related entities.</summary>
/// <typeparam name="T">The type of the domain for this repository.</typeparam>
[PublicAPI]
public class ReadonlyDomainRepository<T> : ReadonlyRepository, IReadonlyDomainRepository<T>
    where T : class
{
    /// <summary>Initializes a new instance of the <see cref="ReadonlyDomainRepository{T}" /> class.</summary>
    /// <param name="domainContext">The domain context to use.</param>
    public ReadonlyDomainRepository(IReadonlyDomainContext<T> domainContext)
        : base(domainContext)
    {
    }

    /// <inheritdoc cref="IReadonlyDomainRepository{T}.Context" />
    public new IReadonlyDomainContext<T> Context => (IReadonlyDomainContext<T>)base.Context;
}
