namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class FatherMapping : FamilyMapping<Father>
{
    public override void Configure(EntityTypeBuilder<Father> builder)
    {
        builder.ToTable("FATHER");
        builder.HasKey(father => father.Id);
        builder.Property(f => f.Id).HasColumnName("ID").ValueGeneratedNever();
        builder.Property(f => f.Name).HasColumnName("NAME");
    }
}
