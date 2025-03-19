namespace DataJam.EntityFrameworkCore.DataContexts;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

/// <summary>Represents a disposable unit of work capable of both read and write operations based on Entity Framework Core's <see cref="DbContext" />.</summary>
[PublicAPI]
public interface IDataContext : DataJam.IDataContext
{
    /// <summary>Gets database related information and operations for this context.</summary>
    DatabaseFacade Database { get; }
}
