namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using System.Data.Entity;

using TestSupport.TestPatterns.Family;

public class MotherMapping : FamilyMapping<Mother>
{
    public MotherMapping()
    {
        ToTable(nameof(Mother));
        HasKey(mother => mother.Id);

        HasMany(mother => mother.Children)
           .WithRequired(child => child.Mother)
           .Map(m => m.MapKey("MotherId"));
    }

    public override void Configure(DbModelBuilder builder)
    {
        builder.Configurations.Add(this);
    }
}
