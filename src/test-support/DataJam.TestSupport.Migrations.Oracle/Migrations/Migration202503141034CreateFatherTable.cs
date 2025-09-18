namespace DataJam.TestSupport.Migrations.Oracle;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 34, "Creates the Father table for Oracle.")]
[UsedImplicitly]
public class Migration202503141034CreateFatherTable : TableMigration
{
    public override string TableName => "FATHER";

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
              .NotNullable();

        Execute.Sql($"CREATE SEQUENCE SEQ_{TableName}_ID START WITH 1 INCREMENT BY 1");
    }
}
