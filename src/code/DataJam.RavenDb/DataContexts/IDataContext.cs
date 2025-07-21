namespace DataJam.RavenDb;

using Raven.Client.Documents.Session;

/// <summary>Represents a disposable unit of work capable of both read and write operations based on RavenDB's <see cref="IDocumentSession" />.</summary>
public interface IDataContext : DataJam.IDataContext
{
}
