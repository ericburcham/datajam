namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class FamilyMapping<T> : IEntityTypeConfiguration<T>
    where T : class
{
    private protected const string SCHEMA = "Family";

    public abstract void Configure(EntityTypeBuilder<T> builder);
}
