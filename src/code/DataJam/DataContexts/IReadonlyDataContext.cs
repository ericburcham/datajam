namespace DataJam;

using System;

/// <summary>Represents a disposable data context which is limited to read operations.</summary>
public interface IReadonlyDataContext : IDataSource, IDisposable
{
}
