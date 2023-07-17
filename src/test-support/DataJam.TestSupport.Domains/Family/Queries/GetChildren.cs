namespace DataJam.TestSupport.Domains.Family;

public class GetChildren : Query<Child>
{
    public GetChildren()
    {
        Selector = dataSource => dataSource.AsQueryable<Child>();
    }
}
