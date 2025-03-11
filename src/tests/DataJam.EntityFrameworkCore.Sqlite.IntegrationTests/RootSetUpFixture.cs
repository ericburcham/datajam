namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using System.IO;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

[SetUpFixture]
public class RootSetUpFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await DeploySqlite().ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public Task OneTimeTearDown()
    {
        SqliteConnection.ClearAllPools();
        var sqlLiteConnectionString = SqliteDependencies.SqliteMockContainer.GetConnectionString();
        var connectionStringBuilder = new SqliteConnectionStringBuilder(sqlLiteConnectionString);
        var path = connectionStringBuilder.DataSource;

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }

    private static async Task DeploySqlite()
    {
        var connectionString = SqliteDependencies.SqliteMockContainer.GetConnectionString();
        var databaseDeployer = new SqliteDatabaseDeployer(connectionString);
        await databaseDeployer.Deploy().ConfigureAwait(false);
    }
}
