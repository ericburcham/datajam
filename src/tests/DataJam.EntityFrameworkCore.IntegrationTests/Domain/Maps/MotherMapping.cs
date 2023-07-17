namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.Domains.Family;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable(nameof(Mother), SCHEMA).HasKey(mother => mother.Id);
    }
}
