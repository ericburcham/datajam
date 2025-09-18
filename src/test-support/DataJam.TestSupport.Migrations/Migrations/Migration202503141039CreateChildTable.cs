namespace DataJam.TestSupport.Migrations;

using System.Collections.Generic;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 39, "Creates the Child table.")]
[UsedImplicitly]
public class Migration202503141039CreateChildTable : TableMigration
{
    public override string TableName => "Child";

    private IEnumerable<TableDescriptor> ForeignKeys
    {
        get
        {
            yield return new("Father");
            yield return new("Mother");
        }
    }

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

        Create.ForeignKeys(this, ForeignKeys);
    }
}
