namespace DataJam;

using JetBrains.Annotations;

/// <summary>Represents a readonly repository for a domain of related entities.</summary>
/// <typeparam name="TDomain">The Type of the domain for this repository.</typeparam>
[PublicAPI]
public interface IReadonlyDomainRepository<in TDomain> : IReadonlyRepository
    where TDomain : class
{
    /// <summary>Gets the domain context.</summary>
    new IReadonlyDomainContext<TDomain> Context { get; }
}
