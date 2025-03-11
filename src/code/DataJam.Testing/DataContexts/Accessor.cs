namespace DataJam.Testing;

using System;

internal class Accessor(Action remover, Func<object, object, object> getter)
{
    internal Func<object, object, object> Getter { get; } = getter;

    internal Action Remover { get; } = remover;
}
