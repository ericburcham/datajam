namespace DataJam.TestSupport.FluentMigrator.Core;

using global::FluentMigrator;

public abstract class TableMigration : Migration, IDescribeTables
{
    public abstract string TableName { get; }
}
