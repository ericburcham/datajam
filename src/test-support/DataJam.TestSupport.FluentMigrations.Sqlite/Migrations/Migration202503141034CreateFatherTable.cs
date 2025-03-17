namespace DataJam.TestSupport.FluentMigrations.Sqlite.Migrations;

using DataJam.TestSupport.FluentMigrator.Core;

using global::FluentMigrator;

[TimestampedMigration(2025, 03, 14, 10, 34, "Creates the Father table.")]
public class Migration202503141034CreateFatherTable : TableMigration
{
    public override string TableName => "Father";

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
