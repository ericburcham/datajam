namespace DataJam.EntityFrameworkCore.Oracle.IntegrationTests.Family;

using System.Linq;

using Microsoft.EntityFrameworkCore;

using TestSupport.TestPatterns.Family;

public class MappingConfigurator : IConfigureDomainMappings<ModelBuilder>
{
    public void Configure(ModelBuilder configurationBinder)
    {
        // Configure Oracle naming convention - convert all identifiers to uppercase
        foreach (var entity in configurationBinder.Model.GetEntityTypes())
        {
            // Convert table names to uppercase
            if (entity.GetTableName() != null)
            {
                entity.SetTableName(entity.GetTableName()!.ToUpperInvariant());
            }

            // Convert column names to uppercase
            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (columnName != null)
                {
                    property.SetColumnName(columnName.ToUpperInvariant());
                }
            }

            // Convert foreign key column names to uppercase
            foreach (var key in entity.GetForeignKeys())
            {
                var columnNames = key.Properties.Select(p => p.GetColumnName()).ToArray();
                for (int i = 0; i < columnNames.Length; i++)
                {
                    if (columnNames[i] != null)
                    {
                        key.Properties[i].SetColumnName(columnNames[i]!.ToUpperInvariant());
                    }
                }
            }
        }

        new ChildMapping().Configure(configurationBinder.Entity<Child>());
        new FatherMapping().Configure(configurationBinder.Entity<Father>());
        new MotherMapping().Configure(configurationBinder.Entity<Mother>());
    }
}
