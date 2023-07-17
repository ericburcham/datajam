namespace DataJam.TestSupport.Domains.Family;

public class Father : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
