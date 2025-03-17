namespace DataJam.TestSupport.FluentMigrator.Core;

using JetBrains.Annotations;

[PublicAPI]
public record TableDescriptor(string TableName) : IDescribeTables;
