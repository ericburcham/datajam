namespace DataJam.TestSupport.Migrations;

using Abstract;

using global::FluentMigrator;

using JetBrains.Annotations;

[TimestampedMigration(2025, 03, 14, 10, 31, "Creates the Family schema.")]
[UsedImplicitly]
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
