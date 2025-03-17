namespace DataJam.TestSupport.FluentMigrator.Deployers;

using System;
using System.Reflection;
using System.Threading.Tasks;

using global::FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

public class SqliteDatabaseDeployer(string connectionString) : DatabaseDeployer
{
    protected override Assembly MigrationAssembly => MigrationAnchor.AnchoredAssembly;

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
        return new ServiceCollection()

               // Add common FluentMigrator services
              .AddFluentMigratorCore()
              .ConfigureRunner(
                   rb => rb

                         // Add SQLite support to FluentMigrator
                        .AddSQLite()

                         // Set the connection string
                        .WithGlobalConnectionString(connectionString)

                         // Define the assembly containing the migrations
                        .ScanIn(migrationAssembly)
                        .For.Migrations())

               // Enable logging to console in the FluentMigrator way
              .AddLogging(lb => lb.AddFluentMigratorConsole())

               // Build the service provider
              .BuildServiceProvider(false);
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
