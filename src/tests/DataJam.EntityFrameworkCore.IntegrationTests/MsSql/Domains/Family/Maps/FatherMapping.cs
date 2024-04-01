namespace DataJam.EntityFrameworkCore.IntegrationTests.MsSql.Domains.Family;

using DataJam.TestSupport.TestPatterns.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FatherMapping : FamilyMapping<Father>
{
    public override void Configure(EntityTypeBuilder<Father> builder)
    {
        builder.ToTable(nameof(Father), SCHEMA).HasKey(father => father.Id);
    }
}
