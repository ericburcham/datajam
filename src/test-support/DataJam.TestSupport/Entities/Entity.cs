namespace DataJam.TestSupport;

public class Entity : Entity<long>, IEntity
{
}

public class Entity<TKey> : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}
