namespace DataJam.TestSupport.Migrators;

using System;
using System.Reflection;
using System.Threading.Tasks;

using DbUp;

public class MySqlDatabaseDeployer : DatabaseDeployer
{
    private readonly string _connectionString;

    public MySqlDatabaseDeployer(string connectionString, string migrationAssembly)
        : base(migrationAssembly)
    {
        _connectionString = connectionString;
    }

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        try
        {
            EnsureDatabase.For.MySqlDatabase(_connectionString);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }

        var upgradeResult = DeployChanges.To.MySqlDatabase(_connectionString).WithScriptsEmbeddedInAssembly(migrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
