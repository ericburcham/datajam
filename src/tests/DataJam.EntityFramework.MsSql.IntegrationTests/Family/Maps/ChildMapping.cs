namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

using System.Data.Entity;

using TestSupport.TestPatterns.Family;

public class ChildMapping : FamilyMapping<Child>
{
    public ChildMapping()
    {
        ToTable(nameof(Child), SCHEMA);
        HasKey(child => child.Id);
    }

    public override void Configure(DbModelBuilder builder)
    {
        builder.Configurations.Add(this);
    }
}
