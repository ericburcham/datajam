namespace DataJam;

using Microsoft.EntityFrameworkCore;

public interface IConfigureDomainMappings
{
    void Configure(ModelBuilder modelBuilder);
}
