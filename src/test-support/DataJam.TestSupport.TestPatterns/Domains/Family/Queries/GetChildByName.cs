namespace DataJam.TestSupport.TestPatterns.Domains.Family;

using System.Linq;

public class GetChildByName : Scalar<Child>
{
    public GetChildByName(string name)
    {
        Selector = dataSource => dataSource.AsQueryable<Child>().Single(child => child.Name == name);
    }
}
