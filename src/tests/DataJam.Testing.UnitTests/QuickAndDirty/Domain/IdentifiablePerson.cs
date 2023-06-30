namespace DataJam.Testing.UnitTests.QuickAndDirty.Domain;

using System;

public class IdentifiablePerson<T> : IIdentifiable<T>
    where T : IEquatable<T>
{
    public T Id { get; set; } = default!;
}
