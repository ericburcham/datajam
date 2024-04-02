namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.TestPatterns.Family;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable(nameof(Mother)).HasKey(mother => mother.Id);
    }
}
