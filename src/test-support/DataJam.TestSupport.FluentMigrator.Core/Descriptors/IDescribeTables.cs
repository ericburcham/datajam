namespace DataJam.TestSupport.FluentMigrator.Core;

using JetBrains.Annotations;

[PublicAPI]
public interface IDescribeTables
{
    public string TableName { get; }
}
