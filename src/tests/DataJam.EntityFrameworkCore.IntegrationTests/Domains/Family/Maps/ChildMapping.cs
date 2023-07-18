namespace DataJam.EntityFrameworkCore.IntegrationTests.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.EntityFrameworkCore.Domains.Family;

public class ChildMapping : FamilyMapping<Child>
{
    public override void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.ToTable(nameof(Child), SCHEMA).HasKey(child => child.Id);
    }
}
