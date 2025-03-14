namespace DataJam.Testing;

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

/// <summary>Provides extension methods for types that extend IEnumerable.</summary>
[PublicAPI]
public static class EnumerableExtensions
{
    /// <summary>Checks to see if the given <paramref name="target" /> is empty.  Returns the opposite of the Any method.</summary>
    /// <param name="target">The enumerable to check.</param>
    /// <typeparam name="T">The element type of the <paramref name="target" />.</typeparam>
    /// <returns>True if the given enumerable has no elements.  Otherwise false.</returns>
    public static bool None<T>(this IEnumerable<T> target)
    {
        return !target.Any();
    }
}
