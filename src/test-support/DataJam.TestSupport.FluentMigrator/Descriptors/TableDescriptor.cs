namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public record TableDescriptor(string TableName) : IDescribeTables;
