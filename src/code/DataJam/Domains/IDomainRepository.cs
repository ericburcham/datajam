namespace DataJam;

public interface IDomainRepository<in TDomain> : IRepository
    where TDomain : class
{
    new IDomainContext<TDomain> Context { get; }
}
