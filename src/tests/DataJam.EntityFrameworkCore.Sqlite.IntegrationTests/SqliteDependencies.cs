namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using TestSupport.Dependencies;
using TestSupport.Dependencies.Sqlite;

[UsedImplicitly]
public class SqliteDependencies
{
    public static DbContextOptions Options
    {
        get
        {
            var sqliteDb = RegisteredTestDependencies.Get<SqliteTestDependency>(DependencyConstants.SQLITE_DEPENDENCY_NAME);

            return new DbContextOptionsBuilder()
                  .UseSqlite(sqliteDb.GetConnectionString())
                  .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning))
                  .Options;
        }
    }
}
