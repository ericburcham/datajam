namespace DataJam.TestSupport.EntityFrameworkCore;

using DataJam.EntityFrameworkCore.Domains;

using Microsoft.EntityFrameworkCore;

public class EFCoreFamilyDomain(DbContextOptions configurationOptions, IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
    : Domain(configurationOptions, mappingConfigurator);
