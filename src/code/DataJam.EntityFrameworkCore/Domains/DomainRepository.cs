namespace DataJam.Domains;

using DataJam.Repositories;

public class DomainRepository<TDomain> : Repository, IDomainRepository<TDomain>
    where TDomain : class
{
    public DomainRepository(IDomainContext<TDomain> domainContext)
        : base(domainContext)
    {
    }

    public new IDomainContext<TDomain> Context => (IDomainContext<TDomain>)base.Context;
}
