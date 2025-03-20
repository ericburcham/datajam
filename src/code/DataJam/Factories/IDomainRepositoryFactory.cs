namespace DataJam.Factories;

using Domains;

using JetBrains.Annotations;

/// <summary>Represents a factory that creates repositories for domains of related entities.</summary>
/// <typeparam name="TDomain">The concrete domain type from the derived implementation.</typeparam>
[PublicAPI]
public interface IDomainRepositoryFactory<in TDomain>
    where TDomain : class
{
    /// <summary>Creates a repository for the specified domain.</summary>
    /// <typeparam name="T">The type of the domain for the new repository.</typeparam>
    /// <returns>A newly created repository for the specified domain.</returns>
    IDomainRepository<T> Create<T>()
        where T : class, TDomain;
}
