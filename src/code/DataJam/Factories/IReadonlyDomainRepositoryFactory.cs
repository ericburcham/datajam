namespace DataJam;

using System;

using JetBrains.Annotations;

/// <summary>Represents a factory that creates readonly repositories for domains of related entities.</summary>
/// <typeparam name="TConfigurationBinder">The concrete type that is used to bind the configuration.</typeparam>
/// <typeparam name="TConfigurationOptions">The concrete type that is used to carry configuration options.</typeparam>
[PublicAPI]
public interface IReadonlyDomainRepositoryFactory<out TConfigurationBinder, in TConfigurationOptions>
    where TConfigurationBinder : class
{
    /// <summary>Creates a readonly repository for the specified domain.</summary>
    /// <typeparam name="T">The type of the domain for the new repository.</typeparam>
    /// <returns>A newly created readonly repository for the specified domain.</returns>
    IReadonlyDomainRepository<T> Create<T>()
        where T : class, IDomain<TConfigurationBinder, TConfigurationOptions>;

    /// <summary>Creates a readonly repository for the specified domain.</summary>
    /// <param name="domainType">The type of the domain for the new repository.</param>
    /// <returns>A newly created readonly repository for the specified domain.</returns>
    IReadonlyRepository Create(Type domainType);
}
