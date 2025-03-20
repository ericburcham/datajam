namespace DataJam.DataContexts;

using System;

using JetBrains.Annotations;

/// <summary>Represents a disposable data context which is limited to read operations.</summary>
[PublicAPI]
public interface IReadonlyDataContext : IDataSource, IDisposable;
