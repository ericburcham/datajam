namespace DataJam.TestSupport.Migrations;

using System.Collections.Generic;

using Abstract;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 39, "Creates the Child table.")]
[UsedImplicitly]
public class Migration202503141039CreateChildTable : FamilyTableMigration
{
    public override string TableName => "Child";

    private IEnumerable<SchemaTableDescriptor> ForeignKeys
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
