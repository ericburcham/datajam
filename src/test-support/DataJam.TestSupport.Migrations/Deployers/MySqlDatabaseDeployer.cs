namespace DataJam.TestSupport.Migrations;

using System;
using System.Reflection;
using System.Threading.Tasks;

using FluentMigrator.Core;

using global::FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

public class MySqlDatabaseDeployer(string connectionString) : DatabaseDeployer
{
    protected override Assembly MigrationAssembly => MigrationAnchor.AnchoredAssembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        EnsureDatabase.For.MySqlDatabase(connectionString);

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
              .ConfigureRunner(rb => rb

                                     // Add MySql support to FluentMigrator
                                    .AddMySql5()

                                     // Set the connection string
                                    .WithGlobalConnectionString(connectionString)

                                     // Define the assembly containing the migrations
                                    .ScanIn(migrationAssembly)
                                    .For.Migrations())
              .AddLogging(lb => lb.AddFluentMigratorConsole())
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
