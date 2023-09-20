namespace DataJam.TestSupport.TestPatterns.Domains.Family;

public class Mother : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
