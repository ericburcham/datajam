namespace DataJam.EntityFramework.MsSql.IntegrationTests;

using System.Data.SqlClient;

using JetBrains.Annotations;

using Testcontainers.MsSql;

using TestSupport.Dependencies;

[UsedImplicitly]
public static class MsSqlDependencies
{
    public static IProvideNameOrConnectionString Options
    {
        get
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            var sqlContainer = RegisteredTestDependencies.Get<MsSqlContainer>(ContainerConstants.MSSQL_CONTAINER_NAME);
            var connectionStringBuilder = new SqlConnectionStringBuilder(sqlContainer.GetConnectionString()) { InitialCatalog = ContainerConstants.MSSQL_TEST_DB };

            return new DbConnectionStringBuilderAdapter(connectionStringBuilder);
        }
    }
}
