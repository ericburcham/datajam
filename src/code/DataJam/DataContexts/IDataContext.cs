namespace DataJam;

using System;

/// <summary>Represents a disposable unit of work capable of both read and write operations.</summary>
public interface IDataContext : IDataSource, IDisposable, IUnitOfWork
{
}
