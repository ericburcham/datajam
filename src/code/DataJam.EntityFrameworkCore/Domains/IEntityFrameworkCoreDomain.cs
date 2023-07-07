namespace DataJam;

using Microsoft.EntityFrameworkCore;

/// <summary>
///     An representation of a domain of related entities that is specific to Entity Framework Core.
/// </summary>
public interface IEntityFrameworkCoreDomain : IDomain<ModelBuilder, DbContextOptions>
{
}
