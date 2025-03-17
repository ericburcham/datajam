namespace DataJam.TestSupport.FluentMigrations;

using Abstract;

using FluentMigrator.Core;

using global::FluentMigrator;

[TimestampedMigration(2025, 03, 14, 10, 38, "Creates the Mother table.")]
public class Migration202503141038CreateMotherTable : FamilyTableMigration
{
    public override string TableName => "Mother";

    public override void Down()
    {
        Delete.Table(TableName).InSchema(SchemaName);
    }

    public override void Up()
    {
        Create.Table(TableName)
              .InSchema(SchemaName)
              .WithDefaultPrimaryKey(TableName)
              .WithDefaultStringColumn("Name");
    }
}
