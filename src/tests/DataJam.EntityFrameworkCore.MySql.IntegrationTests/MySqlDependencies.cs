﻿namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

using global::MySql.Data.MySqlClient;

using Microsoft.EntityFrameworkCore;

using Testcontainers.MySql;

using TestSupport.Dependencies;
using TestSupport.EntityFrameworkCore;

public class MySqlDependencies : Singleton<MySqlDependencies>, IProvideDbContextOptions
{
    public DbContextOptions Options
    {
        get
        {
            var mySqlContainer = RegisteredTestDependencies.Get<MySqlContainer>(ContainerConstants.MYSQL_CONTAINER_NAME);
            var connectionStringBuilder = new MySqlConnectionStringBuilder(mySqlContainer.GetConnectionString()) { Database = ContainerConstants.MYSQL_TEST_DB };

            return new DbContextOptionsBuilder().UseMySQL(connectionStringBuilder.ConnectionString).Options;
        }
    }
}
