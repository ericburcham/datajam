namespace DataJam.EntityFrameworkCore.IntegrationTests.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.EntityFrameworkCore.Domains.Family;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable(nameof(Mother), SCHEMA).HasKey(mother => mother.Id);
    }
}
