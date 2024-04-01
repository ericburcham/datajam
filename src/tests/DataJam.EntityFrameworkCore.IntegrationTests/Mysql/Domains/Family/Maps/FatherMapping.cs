namespace DataJam.EntityFrameworkCore.IntegrationTests.Mysql.Domains.Family.Maps;

using DataJam.TestSupport.TestPatterns.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FatherMapping : FamilyMapping<Father>
{
    public override void Configure(EntityTypeBuilder<Father> builder)
    {
        builder.ToTable(nameof(Father)).HasKey(father => father.Id);
    }
}
