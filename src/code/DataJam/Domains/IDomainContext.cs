namespace DataJam;

using JetBrains.Annotations;

/// <summary>Represents a disposable unit of work that is capable of both read and write operations and which is organized around a domain.</summary>
/// <typeparam name="TDomain">The Type of the domain for this domain context.</typeparam>
[PublicAPI]
public interface IDomainContext<in TDomain> : IDataContext
    where TDomain : class
{
}
