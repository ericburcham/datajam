namespace DataJam;

/// <summary>A class that enforces the Singleton design pattern.</summary>
/// <typeparam name="TDerived">The Type of the derived type.</typeparam>
public abstract class Singleton<TDerived>
    where TDerived : Singleton<TDerived>
{
    private static readonly Lazy<TDerived> LAZY = new(Instantiate);

    /// <summary>Gets the single <typeparamref name="TDerived" /> instance.</summary>
    public static TDerived Instance => LAZY.Value;

    private static TDerived Instantiate()
    {
        return (TDerived)Activator.CreateInstance(typeof(TDerived), true)!;
    }
}
