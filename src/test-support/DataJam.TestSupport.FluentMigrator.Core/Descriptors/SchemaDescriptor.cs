namespace DataJam.TestSupport.FluentMigrator.Core;

using JetBrains.Annotations;

[PublicAPI]
public record SchemaDescriptor(string SchemaName) : IDescribeSchemas;
