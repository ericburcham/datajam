namespace DataJam.TestSupport.EntityFrameworkCore.Domains.Family;

public abstract class Parent : Person
{
    public ICollection<Child> Children { get; } = new List<Child>();
}
