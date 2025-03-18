namespace DataJam.EntityFramework.MySql.IntegrationTests;

using System.Data.Common;

using global::MySql.Data.MySqlClient;

using JetBrains.Annotations;

using Testcontainers.MySql;

using TestSupport.Dependencies;

[UsedImplicitly]
public static class MySqlDependencies
{
    public static DbConnection Options
    {
        get
        {
            var mySqlContainer = RegisteredTestDependencies.Get<MySqlContainer>(ContainerConstants.MYSQL_CONTAINER_NAME);
            var connectionStringBuilder = new MySqlConnectionStringBuilder(mySqlContainer.GetConnectionString()) { Database = ContainerConstants.MYSQL_TEST_DB };

            // Give this a look, too.
            var factory = new global::MySql.Data.EntityFramework.MySqlConnectionFactory();
            return factory.CreateConnection(connectionStringBuilder.ConnectionString);
        }
    }
}
