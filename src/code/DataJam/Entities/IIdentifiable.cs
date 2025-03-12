namespace DataJam;

using System;

using JetBrains.Annotations;

/// <summary>Represents a type that can be clearly identified by its unique <see cref="IIdentifiable{T}.Id" /> property.</summary>
/// <typeparam name="T">The type of the <see cref="IIdentifiable{T}.Id" /> property.</typeparam>
[PublicAPI]
public interface IIdentifiable<T>
    where T : IEquatable<T>
{
    /// <summary>Gets or sets a value that uniquely represents the instance in a system of record.</summary>
    T Id { get; set; }
}
