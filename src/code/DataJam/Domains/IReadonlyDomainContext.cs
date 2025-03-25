namespace DataJam;

using JetBrains.Annotations;

/// <summary>Represents a disposable unit of work that is only capable of read operations.</summary>
/// <typeparam name="T">The Type of the domain for this domain context.</typeparam>
[PublicAPI]
public interface IReadonlyDomainContext<in T> : IReadonlyDataContext
    where T : class;
