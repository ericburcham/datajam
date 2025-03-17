namespace DataJam.TestSupport.FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public abstract class SchemaMigration : Migration, IDescribeSchemas
{
    public abstract string SchemaName { get; }
}
