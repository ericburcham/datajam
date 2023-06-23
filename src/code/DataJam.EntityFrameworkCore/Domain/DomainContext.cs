namespace DataJam.Domain;

using DataContexts;

public class DomainContext<TDomain> : DataContext, IDomainContext<TDomain>
    where TDomain : class, IDomain
{
    public DomainContext(string connectionString, TDomain domain)
        : base(connectionString, domain.MappingConfiguration)
    {
    }
}
