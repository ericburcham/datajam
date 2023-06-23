namespace DataJam.Repositories;

using Domain;

public interface IDomainRepository<in TDomain> : IRepository
    where TDomain : class
{
    // TODO:  Replace this with Context.
    IDomainContext<TDomain> DomainContext { get; }
}
