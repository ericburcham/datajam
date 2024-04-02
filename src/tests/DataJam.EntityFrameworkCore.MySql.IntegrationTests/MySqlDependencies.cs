﻿namespace DataJam.EntityFrameworkCore.MySql.IntegrationTests;

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
    private const string USER_ID = "root";

    private const string PASSWORD = "Password123";

    private readonly ReaderWriterLockSlim _containerLock = new();

    private readonly MySqlContainer _mySql;

    public MySqlDependencies()
    {
        _mySql = new MySqlBuilder().WithUsername(USER_ID).WithPassword(PASSWORD).Build();
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
