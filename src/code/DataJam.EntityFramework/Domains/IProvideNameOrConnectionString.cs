namespace DataJam.EntityFramework;

using JetBrains.Annotations;

/// <summary>Represents a database name or connection string for <see cref="DataContext" /> constructors.</summary>
[PublicAPI]
public interface IProvideNameOrConnectionString
{
    /// <summary>Gets the database name or connection string.</summary>
    string NameOrConnectionString { get; }
}
