namespace DataJam.Repositories;

public interface IDomain
{
    IConfigureDomainMappings MappingConfiguration { get; }
}

public interface IConfigureDomainMappings
{
    void Configure(ModelBuilder modelBuilder)
}
