namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

using System.Data.Entity;

using TestSupport.TestPatterns.Family;

public class FatherMapping : FamilyMapping<Father>
{
    public FatherMapping()
    {
        ToTable(nameof(Father), SCHEMA);
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
