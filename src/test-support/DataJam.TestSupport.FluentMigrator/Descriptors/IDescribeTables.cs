namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public interface IDescribeTables
{
    public string TableName { get; }
}
