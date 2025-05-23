namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using System.Data.Entity;

using TestSupport.TestPatterns.Family;

public class ChildMapping : FamilyMapping<Child>
{
    public ChildMapping()
    {
        ToTable(nameof(Child));
        HasKey(child => child.Id);
    }

    public override void Configure(DbModelBuilder builder)
    {
        builder.Configurations.Add(this);
    }
}
