namespace DataJam.TestSupport.FluentMigrator;

using System.Collections.Generic;
using System.Linq;

using global::FluentMigrator.Builders.Create;
using global::FluentMigrator.Builders.Create.Table;
using global::FluentMigrator.Builders.Delete;

public static class MigrationExtensions
{
    public static void ForeignKeys(this ICreateExpressionRoot create, IDescribeTables migration, IEnumerable<IDescribeTables> foreignKeyTables)
    {
        ForeignKeys(create, migration, foreignKeyTables.ToArray());
    }

    public static void ForeignKeys(this ICreateExpressionRoot create, IDescribeTables migration, params IDescribeTables[] foreignKeyTables)
    {
        foreach (var foreignKeyTable in foreignKeyTables)
        {
            var foreignKeyName = GetDefaultForeignKeyName(migration, foreignKeyTable);
            var defaultForeignColumn = GetDefaultForeignColumn(foreignKeyTable);

            create.ForeignKey(foreignKeyName)
                  .FromTable(migration.TableName)
                  .InSchema(migration.SchemaName)
                  .ForeignColumn(defaultForeignColumn)
                  .ToTable(foreignKeyTable.TableName)
                  .InSchema(foreignKeyTable.SchemaName)
                  .PrimaryColumn("Id");
        }
    }

    public static void ForeignKeys(this IDeleteExpressionRoot create, IDescribeTables migration, IEnumerable<IDescribeTables> foreignKeyTables)
    {
        ForeignKeys(create, migration, foreignKeyTables.ToArray());
    }

    public static void ForeignKeys(this IDeleteExpressionRoot delete, IDescribeTables migration, params IDescribeTables[] foreignKeyTables)
    {
        foreach (var foreignKeyTable in foreignKeyTables)
        {
            var foreignKeyName = GetDefaultForeignKeyName(migration, foreignKeyTable);

            delete.ForeignKey(foreignKeyName)
                  .OnTable(migration.TableName)
                  .InSchema(migration.SchemaName);
        }
    }

    public static ICreateTableWithColumnSyntax WithDefaultBooleanColumn(this ICreateTableWithColumnSyntax syntax, string columnName, bool? defaultValue = null)
    {
        var columnOptionSyntax = syntax.WithColumn(columnName).AsBoolean().NotNullable();

        return defaultValue.HasValue ? columnOptionSyntax.WithDefaultValue(defaultValue.Value) : columnOptionSyntax;
    }

    public static ICreateTableWithColumnSyntax WithDefaultDate(this ICreateTableWithColumnSyntax syntax, string columnName)
    {
        return syntax.WithColumn(columnName).AsDate().NotNullable();
    }

    public static ICreateTableWithColumnSyntax WithDefaultDateTime(this ICreateTableWithColumnSyntax syntax, string columnName)
    {
        return syntax.WithColumn(columnName).AsDateTime().NotNullable();
    }

    public static ICreateTableWithColumnSyntax WithDefaultInt32Column(this ICreateTableWithColumnSyntax syntax, string columnName, int? defaultValue = null)
    {
        var columnOptionSyntax = syntax.WithColumn(columnName).AsInt32().NotNullable();

        return defaultValue.HasValue ? columnOptionSyntax.WithDefaultValue(defaultValue.Value) : columnOptionSyntax;
    }

    public static ICreateTableWithColumnSyntax WithDefaultInt64Column(this ICreateTableWithColumnSyntax syntax, string columnName, long? defaultValue = null)
    {
        var columnOptionSyntax = syntax.WithColumn(columnName).AsInt64().NotNullable();

        return defaultValue.HasValue ? columnOptionSyntax.WithDefaultValue(defaultValue.Value) : columnOptionSyntax;
    }

    public static ICreateTableWithColumnSyntax WithDefaultPrimaryKey(this ICreateTableWithColumnSyntax syntax, string tableName)
    {
        var primaryKeyColumnName = GetDefaultPrimaryKeyName(tableName);

        return syntax.WithColumn("Id").AsInt64().NotNullable().PrimaryKey(primaryKeyColumnName).Identity();
    }

    public static ICreateTableWithColumnSyntax WithDefaultStringColumn(this ICreateTableWithColumnSyntax syntax, string columnName)
    {
        return syntax.WithColumn(columnName).AsString(100).NotNullable();
    }

    private static string GetDefaultForeignColumn(IDescribeTables foreignKeyTable)
    {
        return $"{foreignKeyTable.TableName}Id";
    }

    private static string GetDefaultForeignKeyName(IDescribeTables migration, IDescribeTables foreignKeyTable)
    {
        return $"FK_{migration.TableName}_{foreignKeyTable.TableName}";
    }

    private static string GetDefaultPrimaryKeyName(string tableName)
    {
        return $"PK_{tableName}";
    }
}
