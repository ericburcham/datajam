namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public record SchemaDescriptor(string SchemaName) : IDescribeSchemas;
