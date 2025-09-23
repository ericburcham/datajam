namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

public abstract class FamilyMapping<T> : EntityTypeConfiguration<T>
    where T : class
{
    public abstract void Configure(DbModelBuilder builder);
}
