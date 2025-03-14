namespace DataJam.TestSupport;

using System;

public interface IEntity<TKey> : IIdentifiable<TKey>
    where TKey : IEquatable<TKey>
{
}
