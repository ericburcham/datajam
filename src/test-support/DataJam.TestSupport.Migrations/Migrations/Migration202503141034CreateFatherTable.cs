namespace DataJam.TestSupport.Migrations;

using Abstract;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 34, "Creates the Father table.")]
[UsedImplicitly]
public class Migration202503141034CreateFatherTable : FamilyTableMigration
{
    public override string TableName => "Father";

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
