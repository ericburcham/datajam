﻿namespace DataJam.TestSupport;

using System;

public class Entity<TKey> : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}
