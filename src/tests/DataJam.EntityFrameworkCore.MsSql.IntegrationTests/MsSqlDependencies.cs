namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System;
using System.Threading;

using DotNet.Testcontainers.Containers;

using Microsoft.EntityFrameworkCore;

using Testcontainers.MsSql;

using TestSupport.EntityFrameworkCore;

public class MsSqlDependencies : Singleton<MsSqlDependencies>, IProvideDbContextOptions
{
    private readonly ReaderWriterLockSlim _containerLock = new();

    private readonly MsSqlContainer _msSql = new MsSqlBuilder().Build();

    public DbContextOptions Options => new DbContextOptionsBuilder().UseSqlServer(MsSql.GetConnectionString()).Options;

    internal MsSqlContainer MsSql
    {
        get
        {
            _containerLock.EnterUpgradeableReadLock();

            try
            {
                if (_msSql.State == TestcontainersStates.Undefined)
                {
                    _containerLock.EnterWriteLock();

                    try
                    {
                        _msSql.StartAsync().Wait();
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

            return _msSql;
        }
    }
}
