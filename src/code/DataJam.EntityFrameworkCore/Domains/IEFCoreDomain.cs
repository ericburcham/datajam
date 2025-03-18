// ReSharper disable InconsistentNaming

namespace DataJam.EntityFrameworkCore;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

/// <summary>An representation of a domain of related entities that is specific to Entity Framework Core.</summary>
[PublicAPI]
public interface IEFCoreDomain : IDomain<ModelBuilder, DbContextOptions>;
