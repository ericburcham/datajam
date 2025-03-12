namespace DataJam;

using System;

using JetBrains.Annotations;

/// <summary>A class that enforces the Singleton design pattern.</summary>
/// <typeparam name="T">The Type of the derived type.</typeparam>
[PublicAPI]
public abstract class Singleton<T>
    where T : Singleton<T>
{
    private static readonly Lazy<T> _lazy = new(InstanceFactory);

    /// <summary>Gets the single <typeparamref name="T" /> instance.</summary>
    public static T Instance => _lazy.Value;

    private static T InstanceFactory()
    {
        return (T)Activator.CreateInstance(typeof(T), true)!;
    }
}
