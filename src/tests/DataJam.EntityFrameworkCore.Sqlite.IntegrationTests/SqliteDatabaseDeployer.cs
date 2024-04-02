namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

using TestSupport;

public class SqliteDatabaseDeployer : DatabaseDeployer
{
    private readonly string _connectionString;

    public SqliteDatabaseDeployer(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override Assembly MigrationAssembly => GetType().Assembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        var upgradeResult = DeployChanges.To.SQLiteDatabase(_connectionString).WithScriptsEmbeddedInAssembly(migrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
