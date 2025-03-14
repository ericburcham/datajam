namespace DataJam.EntityFrameworkCore.MsSql.IntegrationTests;

using System;
using System.Reflection;
using System.Threading.Tasks;

using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

using TestSupport;
using TestSupport.FluentMigrator;

public class MsSqlDatabaseDeployer(string connectionString) : DatabaseDeployer
{
    protected override Assembly MigrationAssembly => GetType().Assembly;

    protected override Task DeployInternal(Assembly migrationAssembly)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        using (var serviceProvider = CreateServices())
        {
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        return Task.CompletedTask;

        static ServiceProvider CreateServices()
        {
            var connectionString = MsSqlDependencies.Instance.MsSql.GetConnectionString();

            return new ServiceCollection()

                   // Add common FluentMigrator services
                  .AddFluentMigratorCore()
                  .ConfigureRunner(
                       rb => rb

                             // Add SQLite support to FluentMigrator
                            .AddSqlServer()

                             // Set the connection string
                            .WithGlobalConnectionString(connectionString)

                             // Define the assembly containing the migrations
                            .ScanIn(MigrationAnchor.AnchoredAssembly)
                            .For.Migrations())

                   // Enable logging to console in the FluentMigrator way
                  .AddLogging(lb => lb.AddFluentMigratorConsole())

                   // Build the service provider
                  .BuildServiceProvider(false);
        }
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
