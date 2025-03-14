namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public record TableDescriptor(string SchemaName, string TableName) : SchemaDescriptor(SchemaName), IDescribeTables;
