namespace DataJam.TestSupport.Migrations.Oracle;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 34, "Creates the Father table for Oracle.")]
[UsedImplicitly]
public class Migration202503141034CreateFatherTable : TableMigration
{
    public override string TableName => "Father";

    public override void Down()
    {
        Execute.Sql($"DROP SEQUENCE SEQ_{TableName}_ID");
        Delete.Table(TableName);
    }

    public override void Up()
    {
        Create.Table(TableName)
              .WithColumn("Id")
              .AsInt64()
              .NotNullable()
              .PrimaryKey($"PK_{TableName}")
              .WithDefaultStringColumn("Name");

        // Create sequence for auto-incrementing ID
        Execute.Sql($"CREATE SEQUENCE SEQ_{TableName}_ID START WITH 1 INCREMENT BY 1");

        // Create trigger to auto-populate ID from sequence
        Execute.Sql(
            $@"
            CREATE OR REPLACE TRIGGER TRG_{TableName}_ID
            BEFORE INSERT ON {TableName}
            FOR EACH ROW
            BEGIN
                IF :NEW.Id IS NULL THEN
                    SELECT SEQ_{TableName}_ID.NEXTVAL INTO :NEW.Id FROM DUAL;
                END IF;
            END;
        ");
    }
}
