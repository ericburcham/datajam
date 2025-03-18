namespace DataJam.EntityFramework.MySql.IntegrationTests.Family;

using System.Data.Entity;

using TestSupport.TestPatterns.Family;

public class FatherMapping : FamilyMapping<Father>
{
    public FatherMapping()
    {
        ToTable(nameof(Father));
        HasKey(father => father.Id);

        HasMany(father => father.Children)
           .WithRequired(child => child.Father)
           .Map(m => m.MapKey("FatherId"));
    }

    public override void Configure(DbModelBuilder builder)
    {
        builder.Configurations.Add(this);
    }
}
