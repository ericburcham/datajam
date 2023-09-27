namespace DataJam.EntityFrameworkCore.IntegrationTests.MySql;

using System;
using System.Collections.Generic;
using System.Threading;

using DotNet.Testcontainers.Containers;

using Microsoft.EntityFrameworkCore;

using Testcontainers.MySql;

using TestSupport;
using TestSupport.EntityFrameworkCore;

public class MySqlDependencies : Singleton<MySqlDependencies>, IProvideContainers, IProvideDbContextOptions
{
    private readonly ReaderWriterLockSlim _containerLock = new();

    private readonly MySqlContainer _mySql;

    public MySqlDependencies()
    {
        _mySql = new MySqlBuilder().Build();
        ContainerProvider.Instance.Register(_mySql);
    }

    public IEnumerable<IContainer> Containers
    {
        get
        {
            yield return MySql;
        }
    }

    public DbContextOptions Options => new DbContextOptionsBuilder().UseMySQL(MySql.GetConnectionString()).Options;

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
