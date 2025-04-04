namespace DataJam.TestSupport.TestPatterns.Family;

public class GetChildren : Query<Child>
{
    public GetChildren()
    {
        Selector = dataSource => dataSource.CreateQuery<Child>();
    }
}
