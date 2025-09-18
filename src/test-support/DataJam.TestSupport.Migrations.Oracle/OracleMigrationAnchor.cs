namespace DataJam.TestSupport.Migrations.Oracle;

using System.Reflection;

using JetBrains.Annotations;

[PublicAPI]
public static class OracleMigrationAnchor
{
    public static Assembly AnchoredAssembly => typeof(OracleMigrationAnchor).Assembly;
}
