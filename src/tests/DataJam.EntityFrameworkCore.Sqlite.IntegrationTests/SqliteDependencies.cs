namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using TestSupport.Dependencies;
using TestSupport.Dependencies.Sqlite;
using TestSupport.EntityFrameworkCore;

public class SqliteDependencies : Singleton<SqliteDependencies>, IProvideDbContextOptions
{
    public DbContextOptions Options
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
