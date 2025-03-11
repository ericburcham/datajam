namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class ChildMapping : FamilyMapping<Child>
{
    public override void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.ToTable(nameof(Child), SCHEMA).HasKey(child => child.Id);
    }
}
