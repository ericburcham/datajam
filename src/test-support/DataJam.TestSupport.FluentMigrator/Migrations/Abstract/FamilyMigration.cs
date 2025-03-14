namespace DataJam.TestSupport.FluentMigrator.Migrations.Abstract;

using global::FluentMigrator;

public abstract class FamilyMigration : SchemaMigration
{
    public override string SchemaName => nameof(FamilyMigration)[..(nameof(FamilyMigration).Length - nameof(Migration).Length)];
}
