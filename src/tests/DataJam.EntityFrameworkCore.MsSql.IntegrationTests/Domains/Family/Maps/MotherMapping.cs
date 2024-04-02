namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Domains.Family.Maps;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Domains.Family;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable(nameof(Mother), SCHEMA).HasKey(mother => mother.Id);
    }
}
