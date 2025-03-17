namespace DataJam.TestSupport.FluentMigrations.Sqlite.Migrations;

using FluentMigrator.Core;

using global::FluentMigrator;

[TimestampedMigration(2025, 03, 14, 10, 38, "Creates the Mother table.")]
public class Migration202503141038CreateMotherTable : TableMigration
{
    public override string TableName => "Mother";

    public override void Down()
    {
        Delete.Table(TableName);
    }

    public override void Up()
    {
        Create.Table(TableName)
              .WithDefaultPrimaryKey(TableName)
              .WithDefaultStringColumn("Name");
    }
}
