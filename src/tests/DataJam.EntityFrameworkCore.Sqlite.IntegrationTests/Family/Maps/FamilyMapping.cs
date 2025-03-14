namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class FamilyMapping<T> : IEntityTypeConfiguration<T>
    where T : class
{
    public abstract void Configure(EntityTypeBuilder<T> builder);
}
