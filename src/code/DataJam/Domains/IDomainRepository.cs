namespace DataJam;

using JetBrains.Annotations;

/// <summary>Represents a repository for a domain of related entities.</summary>
/// <typeparam name="TDomain">The Type of the domain for this repository.</typeparam>
[PublicAPI]
public interface IDomainRepository<in TDomain> : IRepository
    where TDomain : class
{
    /// <summary>Gets the domain context.</summary>
    new IDomainContext<TDomain> Context { get; }
}
