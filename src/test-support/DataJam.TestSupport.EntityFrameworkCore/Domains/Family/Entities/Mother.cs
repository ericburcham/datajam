namespace DataJam.TestSupport.EntityFrameworkCore.Domains.Family;

public class Mother : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
