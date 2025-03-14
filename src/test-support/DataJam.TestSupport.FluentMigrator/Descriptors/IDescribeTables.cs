namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public interface IDescribeTables : IDescribeSchemas
{
    public string TableName { get; }
}
