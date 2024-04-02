namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class ChildMapping : FamilyMapping<Child>
{
    public override void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.ToTable(nameof(Child)).HasKey(child => child.Id);
    }
}
