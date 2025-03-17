namespace DataJam.TestSupport.FluentMigrations.Sqlite.Migrations;

using DataJam.TestSupport.FluentMigrator.Core;

using global::FluentMigrator;

[TimestampedMigration(2025, 03, 14, 10, 39, "Creates the Child table.")]
public class Migration202503141039CreateChildTable : TableMigration
{
    public override string TableName => "Child";

    public override void Down()
    {
        Delete.Table(TableName);
    }

    public override void Up()
    {
        Create.Table(TableName)
              .WithDefaultPrimaryKey(TableName)
              .WithDefaultInt64Column("FatherId")
              .WithDefaultInt64Column("MotherId")
              .WithDefaultStringColumn("Name");
    }
}
