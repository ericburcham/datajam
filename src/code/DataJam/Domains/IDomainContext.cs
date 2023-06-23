namespace DataJam.Domains;

public interface IDomainContext<in TDomain> : IDataContext
    where TDomain : class
{
}
