namespace DataJam.EntityFrameworkCore.IntegrationTests.SqlServer;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using DbUp;

using DotNet.Testcontainers.Containers;

using Microsoft.EntityFrameworkCore;

using Testcontainers.MsSql;

using TestSupport;
using TestSupport.EntityFrameworkCore;

public class SqlServerDependencies : Singleton<SqlServerDependencies>, IProvideContainers, IProvideDbContextOptions
{
    private readonly ReaderWriterLockSlim _containerLock = new();

    private readonly MsSqlContainer _msSql;

    private SqlServerDependencies()
    {
        _msSql = new MsSqlBuilder().Build();
        ContainerProvider.Instance.Register(_msSql);
    }

    public IEnumerable<IContainer> Containers
    {
        get
        {
            yield return MsSql;
        }
    }

    public DbContextOptions Options => new DbContextOptionsBuilder().UseSqlServer(MsSql.GetConnectionString()).Options;

    private static Assembly MigrationAssembly => Assembly.Load("DataJam.Migrations");

    private MsSqlContainer MsSql
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

    public Task DeployDatabase()
    {
        var connectionString = Instance.MsSql.GetConnectionString();

        EnsureDatabase.For.SqlDatabase(connectionString);
        var upgradeResult = DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(MigrationAssembly).LogToConsole().Build().PerformUpgrade();

        if (upgradeResult.Successful)
        {
            return Task.CompletedTask;
        }

        throw upgradeResult.Error;
    }
}
