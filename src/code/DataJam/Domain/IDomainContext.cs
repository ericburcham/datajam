namespace DataJam.Domain;

public interface IDomainContext<in TDomain> : IDataContext
    where TDomain : class
{
}
