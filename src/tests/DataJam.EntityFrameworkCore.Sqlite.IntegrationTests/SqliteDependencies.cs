﻿namespace DataJam.EntityFrameworkCore.Sqlite.IntegrationTests;

using System;

using Microsoft.EntityFrameworkCore;

using TestSupport.EntityFrameworkCore;

public class SqliteDependencies : Singleton<SqliteDependencies>, IProvideDbContextOptions
{
    private static readonly Lazy<DbContextOptions> _dbContextOptions = new(BuildDbContextOptions);

    private static readonly Lazy<SqliteMockContainer> _sqlite = new(BuildSqlite);

    public static SqliteMockContainer SqliteMockContainer => _sqlite.Value;

    public TransactionalDbContextOptions Options => new ExplicitTransactionalDbContextOptions(_dbContextOptions.Value, true, true);

    private static DbContextOptions BuildDbContextOptions()
    {
        return new DbContextOptionsBuilder()
              .UseSqlite(SqliteMockContainer.GetConnectionString())
              .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning))
              .Options;
    }

    private static SqliteMockContainer BuildSqlite()
    {
        return new();
    }
}
