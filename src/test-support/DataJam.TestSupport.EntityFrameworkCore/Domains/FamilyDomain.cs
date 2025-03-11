namespace DataJam.TestSupport.EntityFrameworkCore;

using DataJam.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class FamilyDomain(
    DbContextOptions configurationOptions,
    IConfigureDomainMappings<ModelBuilder> mappingConfigurator)
    : EntityFrameworkCoreDomain(configurationOptions, mappingConfigurator);
