namespace DataJam.TestSupport;

public interface IEntity : IEntity<long>
{
}

public interface IEntity<TKey> : IIdentifiable<TKey>
    where TKey : IEquatable<TKey>
{
}
