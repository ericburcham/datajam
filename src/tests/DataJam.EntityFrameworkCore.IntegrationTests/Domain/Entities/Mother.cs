namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

public class Mother : Parent
{
    public void AddChild(Child child)
    {
        Children.Add(child);
    }
}
