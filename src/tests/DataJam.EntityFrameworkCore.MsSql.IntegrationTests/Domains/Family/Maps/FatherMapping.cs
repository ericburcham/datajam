namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Domains.Family.Maps;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Domains.Family;

public class FatherMapping : FamilyMapping<Father>
{
    public override void Configure(EntityTypeBuilder<Father> builder)
    {
        builder.ToTable(nameof(Father), SCHEMA).HasKey(father => father.Id);
    }
}
