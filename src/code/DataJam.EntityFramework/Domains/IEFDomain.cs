namespace DataJam.EntityFramework;

using System.Data.Common;
using System.Data.Entity;

using JetBrains.Annotations;

/// <summary>A representation of a domain of related entities that is specific to Entity Framework.</summary>
[PublicAPI]
public interface IEFDomain : IDomain<DbModelBuilder, IProvideNameOrConnectionString>;
