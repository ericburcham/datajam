namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Testcontainers.MsSql;

using TestSupport.EntityFrameworkCore;
using TestSupport.TestContainers;

public class MsSqlDependencies : Singleton<MsSqlDependencies>, IProvideDbContextOptions
{
    public DbContextOptions Options
    {
        get
        {
            var sqlContainer = RegisteredContainers.Get<MsSqlContainer>(ContainerNames.SQL_SERVER);
            var connectionString = sqlContainer.GetConnectionString();
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "test-db" };
            connectionString = connectionStringBuilder.ConnectionString;

            return new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
        }
    }
}
