namespace DataJam.EntityFrameworkCore.IntegrationTests.Mysql.Domains.Family.Maps;

using DataJam.TestSupport.TestPatterns.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MotherMapping : FamilyMapping<Mother>
{
    public override void Configure(EntityTypeBuilder<Mother> builder)
    {
        builder.ToTable(nameof(Mother)).HasKey(mother => mother.Id);
    }
}
