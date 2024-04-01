namespace DataJam.EntityFrameworkCore.IntegrationTests.MsSql.Domains.Family;

using DataJam.TestSupport.TestPatterns.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable(nameof(Mother), SCHEMA).HasKey(mother => mother.Id);
    }
}
