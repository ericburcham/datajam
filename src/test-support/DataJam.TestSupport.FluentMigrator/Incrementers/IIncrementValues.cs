namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public interface IIncrementValues
{
    /// <summary>Gets the previous value returned by the Increment method.</summary>
    long CurrentValue { get; }

    /// <summary>Gets the next value that will be returned by the Increment method.</summary>
    long NextValue { get; }

    /// <summary>Increments the previously returned value.</summary>
    /// <returns>The previous value of the Increment method, plus the Increment provided in the constructor.</returns>
    long Increment();
}
