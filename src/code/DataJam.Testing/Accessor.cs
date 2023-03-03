namespace DataJam.Testing;

internal class Accessor
{
    public Accessor(Action remover, Func<object, object, object> getter)
    {
        Remover = remover;
        Getter = getter;
    }

    internal Func<object, object, object> Getter { get; }

    internal Action Remover { get; }
}