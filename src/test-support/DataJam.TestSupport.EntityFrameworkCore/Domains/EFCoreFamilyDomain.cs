namespace DataJam.TestSupport.EntityFrameworkCore;

using DataJam.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class EFCoreFamilyDomain(DbContextOptions configurationOptions, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
    : EFCoreDomain(configurationOptions, mappingConfigurator);
