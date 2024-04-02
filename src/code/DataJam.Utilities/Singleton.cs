﻿namespace DataJam;

using System;

/// <summary>A class that enforces the Singleton design pattern.</summary>
/// <typeparam name="TDerived">The Type of the derived type.</typeparam>
public abstract class Singleton<TDerived>
    where TDerived : Singleton<TDerived>
{
    private static readonly Lazy<TDerived> _lazy = new(Instantiate);

    /// <summary>Gets the single <typeparamref name="TDerived" /> instance.</summary>
    public static TDerived Instance => _lazy.Value;

    private static TDerived Instantiate()
    {
        return (TDerived)Activator.CreateInstance(typeof(TDerived), true)!;
    }
}
