namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class FatherMapping : FamilyMapping<Father>
{
    public override void Configure(EntityTypeBuilder<Father> builder)
    {
        builder.ToTable(nameof(Father), SCHEMA).HasKey(father => father.Id);
    }
}
