namespace DataJam;

using Microsoft.EntityFrameworkCore;

public interface IEntityFrameworkCoreDomain : IDomain<ModelBuilder, DbContextOptions>
{
}
