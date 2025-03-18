namespace DataJam.TestSupport.Migrations;

using Abstract;

using global::FluentMigrator;

[TimestampedMigration(2025, 03, 14, 10, 31, "Creates the Family schema.")]
public class Migration202503141031CreateFamilySchema : FamilyMigration
{
    public override void Down()
    {
        Delete.Schema(SchemaName);
    }

    public override void Up()
    {
        Create.Schema(SchemaName);
    }
}
