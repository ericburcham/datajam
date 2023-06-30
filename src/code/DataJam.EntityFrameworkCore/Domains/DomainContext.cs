namespace DataJam;

using Microsoft.EntityFrameworkCore;

public class DomainContext<TDomain> : DataContext, IDomainContext<TDomain>
    where TDomain : class, IDomain
{
    public DomainContext(DbContextOptions options, TDomain domain)
        : base(options, domain.MappingConfigurator)
    {
    }
}
