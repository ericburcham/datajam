namespace DataJam.TestSupport.FluentMigrator.Migrations;

using System.Collections.Generic;

using Abstract;

using global::FluentMigrator;

[TimestampedMigration(2025, 03, 14, 10, 39, "Creates the Child table.")]
public class Migration202503141039CreateChildTable : FamilyTableMigration
{
    public override string TableName => "Child";

    private IEnumerable<TableDescriptor> ForeignKeys
    {
        get
        {
            yield return new(SchemaName, "Father");
            yield return new(SchemaName, "Mother");
        }
    }

    public override void Down()
    {
        Delete.Table(TableName).InSchema(SchemaName);
    }

    public override void Up()
    {
        Create.Table(TableName)
              .InSchema(SchemaName)
              .WithDefaultPrimaryKey(TableName)
              .WithDefaultInt64Column("FatherId")
              .WithDefaultInt64Column("MotherId")
              .WithDefaultStringColumn("Name");

        Create.ForeignKeys(this, ForeignKeys);
    }
}
