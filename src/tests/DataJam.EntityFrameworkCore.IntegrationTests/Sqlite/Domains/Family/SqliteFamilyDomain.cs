namespace DataJam.EntityFrameworkCore.IntegrationTests.Sqlite.Domains.Family;

using Microsoft.EntityFrameworkCore;

public class SqliteFamilyDomain : EntityFrameworkCoreDomain
{
    public SqliteFamilyDomain(DbContextOptions configurationOptions, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
        : base(configurationOptions, mappingConfigurator)
    {
    }
}
