namespace DataJam.EntityFrameworkCore.IntegrationTests.Mysql;

using System;
using System.Collections.Generic;
using System.Threading;

using DataJam.TestSupport;
using DataJam.TestSupport.EntityFrameworkCore;

using DotNet.Testcontainers.Containers;

using Microsoft.EntityFrameworkCore;

using MySql.Data.MySqlClient;

using Testcontainers.MySql;

public class MySqlDependencies : Singleton<MySqlDependencies>, IProvideContainers, IProvideDbContextOptions
{
    private const string USER_ID = "root";

    private const string PASSWORD = "Password123";

    private readonly ReaderWriterLockSlim _containerLock = new();

    private readonly MySqlContainer _mySql;

    public MySqlDependencies()
    {
        _mySql = new MySqlBuilder().WithUsername(USER_ID).WithPassword(PASSWORD).Build();
        ContainerProvider.Instance.Register(_mySql);
    }

    public IEnumerable<IContainer> Containers
    {
        get
        {
            yield return MySql;
        }
    }

    public DbContextOptions Options
    {
        get
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            var connectionString = MySql.GetConnectionString();
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString) { UserID = USER_ID, Password = PASSWORD };
            var contextOptionsBuilder = dbContextOptionsBuilder.UseMySQL(connectionStringBuilder.ConnectionString);

            return contextOptionsBuilder.Options;
        }
    }

    internal MySqlContainer MySql
    {
        get
        {
            _containerLock.EnterUpgradeableReadLock();

            try
            {
                if (_mySql.State == TestcontainersStates.Undefined)
                {
                    _containerLock.EnterWriteLock();

                    try
                    {
                        _mySql.StartAsync().Wait();
                    }
                    finally
                    {
                        _containerLock.ExitWriteLock();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _containerLock.ExitUpgradeableReadLock();
            }

            return _mySql;
        }
    }
}
