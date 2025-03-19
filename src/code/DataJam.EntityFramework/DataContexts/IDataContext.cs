namespace DataJam.EntityFramework.DataContexts;

using System.Data.Entity;

using JetBrains.Annotations;

/// <summary>Represents a disposable unit of work capable of both read and write operations based on Entity Framework's <see cref="DbContext" />.</summary>
[PublicAPI]
public interface IDataContext : DataJam.DataContexts.IDataContext
{
    /// <summary>Gets a Database instance for this context that allows for creation/deletion/existence checks for the underlying database.</summary>
    Database Database { get; }
}
