namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public interface IDescribeSchemas
{
    public string SchemaName { get; }
}
