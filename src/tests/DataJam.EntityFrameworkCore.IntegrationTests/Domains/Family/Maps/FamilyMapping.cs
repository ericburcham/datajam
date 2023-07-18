namespace DataJam.EntityFrameworkCore.IntegrationTests.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class FamilyMapping<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    private protected const string SCHEMA = "Family";

    public abstract void Configure(EntityTypeBuilder<TEntity> builder);
}
