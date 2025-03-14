namespace DataJam.TestSupport.FluentMigrator.Migrations.Abstract;

using JetBrains.Annotations;

[PublicAPI]
public abstract class TableMigration : SchemaMigration, IDescribeTables
{
    public abstract string TableName { get; }
}
