namespace DataJam;

using Microsoft.EntityFrameworkCore;

public interface IConfigureDomainMappings
{
    void ApplyDomainMappings(ModelBuilder modelBuilder);
}
