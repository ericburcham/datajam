namespace DataJam.EntityFrameworkCore.Domains;

using DataJam.Domains;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

/// <summary>A representation of a domain of related entities that is specific to Entity Framework Core.</summary>
[PublicAPI]
public interface IDomain : IDomain<ModelBuilder, DbContextOptions>;
