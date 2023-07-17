namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

public class GetChildren : Query<Child>
{
    public GetChildren()
    {
        Selector = dataSource => dataSource.AsQueryable<Child>();
    }
}
