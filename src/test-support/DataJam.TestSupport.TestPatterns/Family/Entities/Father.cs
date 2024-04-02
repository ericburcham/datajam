namespace DataJam.TestSupport.TestPatterns.Family;

public class Father : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
