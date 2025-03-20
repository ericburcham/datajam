namespace DataJam.DataContexts;

using System;

using JetBrains.Annotations;

/// <summary>Represents a disposable unit of work capable of both read and write operations.</summary>
[PublicAPI]
public interface IDataContext : IDataSource, IDisposable, IUnitOfWork;
