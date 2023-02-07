namespace DataJam.InMemory.UnitTests.Domains.Family;

public class Person<TId> : Entity<TId> where TId : IEquatable<TId>
{
    public string Name { get; set; } = null!;
}