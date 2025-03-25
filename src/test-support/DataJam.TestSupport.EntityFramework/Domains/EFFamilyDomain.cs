namespace DataJam.TestSupport.EntityFramework;

using System.Data.Entity;

using DataJam.EntityFramework;

public class EFFamilyDomain(IProvideNameOrConnectionString configurationOptions, IConfigureDomainMappings<DbModelBuilder> mappingConfigurator)
    : Domain(configurationOptions, mappingConfigurator);
