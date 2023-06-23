namespace DataJam;

using System;

// TODO:  Determine if this is even needed, here.

/// <summary>Represents a type that can be clearly identified by its unique <see cref="IIdentifiable{T}.Id" /> property.</summary>
/// <typeparam name="TId">The type of the <see cref="IIdentifiable{TId}.Id" /> property.</typeparam>
public interface IIdentifiable<TId>
    where TId : IEquatable<TId>
{
    /// <summary>Gets or sets a value that uniquely represents the instance in a system of record.</summary>
    TId Id { get; set; }
}
