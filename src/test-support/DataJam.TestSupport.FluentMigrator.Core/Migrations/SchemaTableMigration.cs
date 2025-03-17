namespace DataJam.TestSupport.FluentMigrator.Core;

using JetBrains.Annotations;

[PublicAPI]
public abstract class SchemaTableMigration : SchemaMigration, IDescribeTables
{
    public abstract string TableName { get; }
}
