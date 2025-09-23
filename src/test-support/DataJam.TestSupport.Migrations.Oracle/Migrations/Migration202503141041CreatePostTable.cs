namespace DataJam.TestSupport.Migrations.Oracle;

using System.Collections.Generic;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 41, "Creates the Post table for Oracle.")]
[UsedImplicitly]
public class Migration202503141041CreatePostTable : TableMigration
{
    public override string TableName => "POST";

    private IEnumerable<TableDescriptor> ForeignKeys
    {
        get
        {
            yield return new("BLOG");
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
              .WithColumn("TITLE")
              .AsString(100)
              .Nullable()
              .WithColumn("CONTENT")
              .AsString(1000)
              .Nullable()
              .WithColumn("BLOGID")
              .AsInt64()
              .NotNullable();

        Create.ForeignKeys(this, ForeignKeys);

        Execute.Sql($"CREATE SEQUENCE SEQ_{TableName}_ID START WITH 1 INCREMENT BY 1");
    }
}
