namespace DataJam.EntityFrameworkCore.IntegrationTests.Mysql.Domains.Family.Maps;

using DataJam.TestSupport.TestPatterns.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChildMapping : FamilyMapping<Child>
{
    public override void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.ToTable(nameof(Child)).HasKey(child => child.Id);
    }
}
