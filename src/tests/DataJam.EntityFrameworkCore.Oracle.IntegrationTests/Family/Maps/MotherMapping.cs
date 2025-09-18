namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable("MOTHER");
        builder.HasKey(mother => mother.Id);
        builder.Property(m => m.Id).HasColumnName("ID").ValueGeneratedNever();
        builder.Property(m => m.Name).HasColumnName("NAME");
    }
}
