namespace DataJam.TestSupport.Migrations;

using System;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

using FluentMigrator.Core;

using global::FluentMigrator.Runner;

using global::Oracle.ManagedDataAccess.Client;

using Microsoft.Extensions.DependencyInjection;

public class OracleDatabaseDeployer(string connectionString) : DatabaseDeployer
{
    static OracleDatabaseDeployer()
    {
        // Register Oracle provider factory VERY early to ensure it's available
        // This must happen before FluentMigrator tries to resolve providers
        try
        {
            DbProviderFactories.RegisterFactory("Oracle.ManagedDataAccess.Client", OracleClientFactory.Instance);
        }
        catch
        {
            // Factory might already be registered, ignore
        }
    }

    protected override Assembly MigrationAssembly => Oracle.OracleMigrationAnchor.AnchoredAssembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        using (var serviceProvider = BuildServiceProvider(connectionString, migrationAssembly))
        {
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        return Task.CompletedTask;
    }

    private static ServiceProvider BuildServiceProvider(string connectionString, Assembly migrationAssembly)
    {
        var services = new ServiceCollection();

        // Add FluentMigrator core services
        services.AddFluentMigratorCore();

        // Configure the runner with Oracle Managed explicitly
        services.ConfigureRunner(rb => rb
            .AddOracleManaged()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(migrationAssembly)
            .For.Migrations());

        // Add logging
        services.AddLogging(lb => lb.AddFluentMigratorConsole());

        // Explicitly replace any Oracle-related services to ensure we use managed provider
        services.AddSingleton<global::FluentMigrator.Runner.Processors.Oracle.OracleManagedDbFactory>();

        return services.BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        try
        {
            runner.MigrateUp();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
    }
}
