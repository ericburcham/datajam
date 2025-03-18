namespace DataJam.TestSupport.EntityFramework;

using System.Data.Common;
using System.Data.Entity;

using DataJam.EntityFramework;

public class EFFamilyDomain(IProvideNameOrConnectionString configurationOptions, IConfigureDomainMappings<DbModelBuilder> mappingConfigurator)
    : EFDomain(configurationOptions, mappingConfigurator);
