namespace DataJam.Domains;

using DataJam.DataContexts;

using Domain;

using Microsoft.EntityFrameworkCore;

public class DomainContext<TDomain> : DataContext, IDomainContext<TDomain>
    where TDomain : class, IDomain
{
    public DomainContext(DbContextOptions options, TDomain domain)
        : base(options, domain.MappingConfiguration)
    {
    }
}
