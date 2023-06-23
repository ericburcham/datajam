namespace DataJam;

public interface IDomainContext<in TDomain> : IDataContext
    where TDomain : class
{
}
