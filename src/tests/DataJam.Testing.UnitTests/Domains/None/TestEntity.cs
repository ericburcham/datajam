namespace DataJam.Testing.UnitTests.Domains.None;

using System;

public class TestEntity<T> : Entity<T>
    where T : IEquatable<T>;
