namespace DataJam.TestSupport.FluentMigrations.Abstract;

using FluentMigrator.Core;

public abstract class FamilyTableMigration : FamilyMigration, IDescribeSchemaTables
{
    public abstract string TableName { get; }
}
