namespace DataJam.RavenDb;

using Raven.Client.Documents.Session;

/// <summary>Represents a disposable unit of work capable of both read and write operations based on RavenDB's <see cref="IAsyncDocumentSession" />.</summary>
public interface IAsyncDataContext : DataJam.IDataContext
{
}
