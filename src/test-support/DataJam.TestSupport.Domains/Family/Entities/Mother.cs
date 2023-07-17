namespace DataJam.TestSupport.Domains.Family;

public class Mother : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
