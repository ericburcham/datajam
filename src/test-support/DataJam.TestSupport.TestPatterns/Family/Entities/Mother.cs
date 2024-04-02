namespace DataJam.TestSupport.TestPatterns.Family;

public class Mother : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
