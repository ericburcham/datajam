namespace DataJam.TestSupport.Migrations.Oracle;

using System.Collections.Generic;

using FluentMigrator.Core;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 39, "Creates the Child table for Oracle.")]
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
              .WithColumn("Id")
              .AsInt64()
              .NotNullable()
              .PrimaryKey($"PK_{TableName}")
              .WithDefaultStringColumn("Name")
              .WithDefaultInt64Column("FatherId")
              .WithDefaultInt64Column("MotherId");

        Create.ForeignKeys(this, ForeignKeys);

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
