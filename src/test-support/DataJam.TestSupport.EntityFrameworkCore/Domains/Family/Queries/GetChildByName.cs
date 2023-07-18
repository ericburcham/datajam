namespace DataJam.TestSupport.EntityFrameworkCore.Domains.Family;

public class GetChildByName : Scalar<Child>
{
    public GetChildByName(string name)
    {
        Selector = dataSource => dataSource.AsQueryable<Child>().Single(child => child.Name == name);
    }
}
