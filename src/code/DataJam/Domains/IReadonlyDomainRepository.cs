namespace DataJam.Domains;

using JetBrains.Annotations;

/// <summary>Represents a readonly repository for a domain of related entities.</summary>
/// <typeparam name="T">The Type of the domain for this repository.</typeparam>
[PublicAPI]
public interface IReadonlyDomainRepository<in T> : IReadonlyRepository
    where T : class
{
    /// <summary>Gets the domain context.</summary>
    new IReadonlyDomainContext<T> Context { get; }
}
