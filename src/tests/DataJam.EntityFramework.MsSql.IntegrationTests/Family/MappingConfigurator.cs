namespace DataJam.EntityFramework.MsSql.IntegrationTests.Family;

using System.Data.Entity;

public class MappingConfigurator : IConfigureDomainMappings<DbModelBuilder>
{
    public void Configure(DbModelBuilder configurationBinder)
    {
        new ChildMapping().Configure(configurationBinder);
        new FatherMapping().Configure(configurationBinder);
        new MotherMapping().Configure(configurationBinder);
    }
}
