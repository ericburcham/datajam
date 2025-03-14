namespace DataJam.TestSupport.TestPatterns.Family;

public abstract class Person : Entity<long>
{
    public string Name { get; init; } = null!;
}
