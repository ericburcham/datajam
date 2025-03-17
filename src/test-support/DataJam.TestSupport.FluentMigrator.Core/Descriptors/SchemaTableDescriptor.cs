namespace DataJam.TestSupport.FluentMigrator.Core;

using JetBrains.Annotations;

[PublicAPI]
public record SchemaTableDescriptor(string SchemaName, string TableName) : IDescribeSchemaTables;
