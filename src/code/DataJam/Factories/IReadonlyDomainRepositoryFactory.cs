namespace DataJam;

using JetBrains.Annotations;

/// <summary>Represents a factory that creates readonly repositories for domains of related entities.</summary>
/// <typeparam name="TDomain">The concrete domain type from the derived implementation.</typeparam>
[PublicAPI]
public interface IReadonlyDomainRepositoryFactory<in TDomain>
    where TDomain : class
{
    /// <summary>Creates a readonly repository for the specified domain.</summary>
    /// <typeparam name="T">The type of the domain for the new repository.</typeparam>
    /// <returns>A newly created readonly repository for the specified domain.</returns>
    IReadonlyDomainRepository<T> Create<T>()
        where T : class, TDomain;
}
