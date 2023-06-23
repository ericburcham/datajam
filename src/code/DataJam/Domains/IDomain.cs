namespace DataJam.Domain;

using Microsoft.EntityFrameworkCore;

public interface IDomain
{
    IConfigureDomainMappings MappingConfiguration { get; }
}

public interface IConfigureDomainMappings
{
    void ApplyDomainMappings(ModelBuilder modelBuilder);
}
