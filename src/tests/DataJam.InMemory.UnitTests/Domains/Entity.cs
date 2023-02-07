namespace DataJam.InMemory.UnitTests.Domains;

public class Entity<T> : IIdentifyThings<T> where T : IEquatable<T>
{
    public T Id { get; set; } = default!;
}