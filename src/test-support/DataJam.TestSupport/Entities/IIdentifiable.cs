namespace DataJam.TestSupport.Entities;

public interface IIdentifiable : IIdentifiable<long>
{
}

public interface IIdentifiable<TKey>
    where TKey : IEquatable<TKey>
{
    TKey Id { get; set; }
}
