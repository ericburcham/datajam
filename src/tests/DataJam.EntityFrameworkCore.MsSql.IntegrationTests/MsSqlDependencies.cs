namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Testcontainers.MsSql;

using TestSupport.Dependencies;
using TestSupport.EntityFrameworkCore;

public class MsSqlDependencies : Singleton<MsSqlDependencies>, IProvideDbContextOptions
{
    public DbContextOptions Options
    {
        get
        {
            var sqlContainer = RegisteredTestDependencies.Get<MsSqlContainer>(ContainerConstants.MSSQL_CONTAINER_NAME);
            var connectionStringBuilder = new SqlConnectionStringBuilder(sqlContainer.GetConnectionString()) { InitialCatalog = ContainerConstants.MSSQL_TEST_DB };

            return new DbContextOptionsBuilder().UseSqlServer(connectionStringBuilder.ConnectionString).Options;
        }
    }
}
