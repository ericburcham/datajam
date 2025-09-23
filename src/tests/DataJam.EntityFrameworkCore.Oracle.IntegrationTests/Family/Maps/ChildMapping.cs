namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class ChildMapping : FamilyMapping<Child>
{
    public override void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.ToTable("CHILD");
        builder.HasKey(child => child.Id);
        builder.Property(c => c.FatherId).HasColumnName("FATHERID").ValueGeneratedNever();
        builder.Property(c => c.MotherId).HasColumnName("MOTHERID").ValueGeneratedNever();
        builder.Property(c => c.Id).HasColumnName("ID").ValueGeneratedNever();
        builder.Property(c => c.Name).HasColumnName("NAME");
    }
}
