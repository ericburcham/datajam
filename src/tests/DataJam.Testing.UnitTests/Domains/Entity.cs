namespace DataJam.Testing.UnitTests.Domains;

public abstract class Entity<T> : IIdentifiable<T> where T : IEquatable<T>
{
    public T Id { get; set; } = default!;
}