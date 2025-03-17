namespace DataJam.TestSupport.FluentMigrator.Migrations.Abstract;

using JetBrains.Annotations;

[PublicAPI]
public abstract class SchemaTableMigration : SchemaMigration, IDescribeTables
{
    public abstract string TableName { get; }
}
