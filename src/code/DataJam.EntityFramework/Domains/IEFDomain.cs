namespace DataJam.EntityFramework;

using System.Data.Common;
using System.Data.Entity;

using JetBrains.Annotations;

/// <summary>A representation of a domain of related entities that is specific to Entity Framework.</summary>
[PublicAPI]
public interface IEFDomain : IDomain<DbModelBuilder, IProvideNameOrConnectionString>;

#pragma warning disable SA1600
[PublicAPI]
public interface IEFDbConnectionDomain : IDomain<DbModelBuilder, DbConnection>;
#pragma warning restore SA1600
