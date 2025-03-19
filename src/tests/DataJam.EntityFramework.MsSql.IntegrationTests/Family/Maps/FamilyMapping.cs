namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

public abstract class FamilyMapping<T> : EntityTypeConfiguration<T>
    where T : class
{
    private protected const string SCHEMA = "Family";

    public abstract void Configure(DbModelBuilder builder);
}
