namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public record SchemaTableDescriptor(string SchemaName, string TableName) : IDescribeSchemaTables;
