namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable(nameof(Mother), SCHEMA).HasKey(mother => mother.Id);
    }
}
