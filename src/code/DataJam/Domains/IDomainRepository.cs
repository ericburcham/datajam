namespace DataJam;

using JetBrains.Annotations;

/// <summary>Represents a repository for a domain of related entities.</summary>
/// <typeparam name="T">The Type of the domain for this repository.</typeparam>
[PublicAPI]
public interface IDomainRepository<in T> : IRepository
    where T : class
{
    /// <summary>Gets the domain context.</summary>
    new IDomainContext<T> Context { get; }
}
