namespace DataJam.TestSupport.EntityFramework;

using System.Data.Entity;

using DataJam.EntityFramework;

public class EFFamilyDomain(IProvideNameOrConnectionString configurationOptions, IConfigureDomainMappings<DbModelBuilder> mappingConfigurator)
    : EFDomain(configurationOptions, mappingConfigurator);
