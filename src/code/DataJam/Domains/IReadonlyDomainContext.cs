namespace DataJam;

/// <summary>Represents a disposable unit of work that is only capable of read operations.</summary>
/// <typeparam name="TDomain">The Type of the domain for this domain context.</typeparam>
public interface IReadonlyDomainContext<in TDomain> : IReadonlyDataContext
    where TDomain : class
{
}
