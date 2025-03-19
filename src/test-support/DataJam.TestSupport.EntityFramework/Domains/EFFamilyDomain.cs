namespace DataJam.TestSupport.EntityFramework;

using System.Data.Entity;

using DataJam.EntityFramework.Configuration;
using DataJam.EntityFramework.Domains;

using Domains;

public class EFFamilyDomain(IProvideNameOrConnectionString configurationOptions, IConfigureDomainMappings<DbModelBuilder> mappingConfigurator)
    : Domain(configurationOptions, mappingConfigurator);
