namespace DataJam.TestSupport.Migrations.Oracle;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 38, "Creates the Mother table for Oracle.")]
[UsedImplicitly]
public class Migration202503141038CreateMotherTable : TableMigration
{
    public override string TableName => "MOTHER";

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
