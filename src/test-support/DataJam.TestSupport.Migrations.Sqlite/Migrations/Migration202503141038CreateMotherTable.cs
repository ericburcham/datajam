namespace DataJam.TestSupport.Migrations.Sqlite.Migrations;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 38, "Creates the Mother table.")]
[UsedImplicitly]
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
