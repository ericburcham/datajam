namespace DataJam.TestSupport.FluentMigrator.Migrations.Abstract;

using global::FluentMigrator;

public abstract class TableMigration : Migration, IDescribeTables
{
    public abstract string TableName { get; }
}
