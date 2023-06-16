namespace DataJam.Repositories;

using Domain;

public interface IDomainRepository<in TDomain> : IRepository
    where TDomain : class
{
    IDomainContext<TDomain> DomainContext { get; }
}
