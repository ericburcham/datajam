namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

public class Father : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
