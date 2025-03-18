namespace DataJam.EntityFramework.MySql.IntegrationTests;

using Configuration;

using global::MySql.Data.MySqlClient;

using JetBrains.Annotations;

using Testcontainers.MySql;

using TestSupport.Dependencies;

[UsedImplicitly]
public static class MySqlDependencies
{
    public static IProvideNameOrConnectionString Options
    {
        get
        {
            var mySqlContainer = RegisteredTestDependencies.Get<MySqlContainer>(ContainerConstants.MYSQL_CONTAINER_NAME);
            var connectionStringBuilder = new MySqlConnectionStringBuilder(mySqlContainer.GetConnectionString()) { Database = ContainerConstants.MYSQL_TEST_DB };

            return new DbConnectionStringBuilderAdapter(connectionStringBuilder);
        }
    }
}
