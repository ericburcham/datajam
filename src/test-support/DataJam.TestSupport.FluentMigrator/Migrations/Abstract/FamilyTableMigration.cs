namespace DataJam.TestSupport.FluentMigrator.Migrations.Abstract;

public abstract class FamilyTableMigration : FamilyMigration, IDescribeTables
{
    public abstract string TableName { get; }
}
