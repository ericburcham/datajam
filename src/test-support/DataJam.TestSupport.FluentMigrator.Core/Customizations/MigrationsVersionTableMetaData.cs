#pragma warning disable CS0618 // This compiler warning comes from inheriting the default constructor in the base class, which is obsolete.
namespace DataJam.TestSupport.FluentMigrator.Core;

using global::FluentMigrator.Runner.VersionTableInfo;

using JetBrains.Annotations;

[PublicAPI]
[VersionTableMetaData]
public class MigrationsVersionTableMetaData : DefaultVersionTableMetaData
{
    public override string SchemaName => "Migrations";
}
