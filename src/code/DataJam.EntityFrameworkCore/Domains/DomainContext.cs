namespace DataJam;

public class DomainContext<TDomain> : DataContext, IDomainContext<TDomain>
    where TDomain : class, IEntityFrameworkCoreDomain
{
    public DomainContext(TDomain domain)
        : base(domain.ConfigurationOptions, domain.MappingConfigurator)
    {
    }
}
