namespace DataJam.TestSupport.Migrators;

using System.Reflection;
using System.Threading.Tasks;

using DbUp;

public class SqliteDatabaseDeployer : DatabaseDeployer
{
    private readonly string _connectionString;

    public SqliteDatabaseDeployer(string connectionString, string migrationAssembly)
        : base(migrationAssembly)
    {
        _connectionString = connectionString;
    }

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
