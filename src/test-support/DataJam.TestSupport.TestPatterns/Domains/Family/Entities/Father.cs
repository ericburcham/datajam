namespace DataJam.TestSupport.TestPatterns.Domains.Family;

public class Father : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
