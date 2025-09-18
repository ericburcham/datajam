namespace DataJam.TestSupport.Migrations.Oracle;

using System.Collections.Generic;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 39, "Creates the Child table for Oracle.")]
[UsedImplicitly]
public class Migration202503141039CreateChildTable : TableMigration
{
    public override string TableName => "CHILD";

    private IEnumerable<TableDescriptor> ForeignKeys
    {
        get
        {
            yield return new("FATHER");
            yield return new("MOTHER");
        }
    }

    public override void Down()
    {
        Execute.Sql($"DROP SEQUENCE SEQ_{TableName}_ID");
        Delete.Table(TableName);
    }

    public override void Up()
    {
        Create.Table(TableName)
              .WithColumn("ID")
              .AsInt64()
              .NotNullable()
              .PrimaryKey($"PK_{TableName}")
              .WithColumn("NAME")
              .AsString(100)
              .NotNullable()
              .WithColumn("FATHERID")
              .AsInt64()
              .NotNullable()
              .WithColumn("MOTHERID")
              .AsInt64()
              .NotNullable();

        Create.ForeignKeys(this, ForeignKeys);

        Execute.Sql($"CREATE SEQUENCE SEQ_{TableName}_ID START WITH 1 INCREMENT BY 1");
    }
}
