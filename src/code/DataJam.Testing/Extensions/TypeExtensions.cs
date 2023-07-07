namespace DataJam.Testing;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>Provides extension methods for <see cref="Type" />s.</summary>
public static class TypeExtensions
{
    /// <summary>Determines if the given <paramref name="type" /> is an enumerable of some kind.</summary>
    /// <param name="type">The <see cref="Type" /> to examine.</param>
    /// <returns>A value indicating whether then given <paramref name="type" /> is <see cref="IEnumerable" />.</returns>
    public static bool IsEnumerable(this Type type)
    {
        if (type == typeof(string))
        {
            return false;
        }

        return type == typeof(IEnumerable) || type.GetInterfaces().Contains(typeof(IEnumerable));
    }

    /// <summary>
    ///     Returns the type param "T" from a <paramref name="type" /> that is <see cref="IEnumerable{T}" />.  If the given type is not enumerable, this method returns
    ///     the given type.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> to examine.</param>
    /// <returns>A <see cref="Type" /> representing the element type if <paramref name="type" /> is an enumerable.  Otherwise the given type.</returns>
    public static Type ToSingleType(this Type type)
    {
        return type.IsGenericType && type.IsEnumerable() ? type.GetGenericArguments().Single() : type;
    }
}
