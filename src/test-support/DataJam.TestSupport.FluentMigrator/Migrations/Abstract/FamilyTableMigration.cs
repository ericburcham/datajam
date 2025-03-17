namespace DataJam.TestSupport.FluentMigrator.Migrations.Abstract;

public abstract class FamilyTableMigration : FamilyMigration, IDescribeSchemaTables
{
    public abstract string TableName { get; }
}
