namespace DataJam;

/// <summary>
///     Defines an Id property for identifying individual objects.
/// </summary>
/// <typeparam name="TId">The <see cref="Type" /> of the Id.</typeparam>
public interface IIdentifyThings<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    ///     Gets or sets a value identifying the individual object.
    /// </summary>
    public TId Id { get; set; }
}