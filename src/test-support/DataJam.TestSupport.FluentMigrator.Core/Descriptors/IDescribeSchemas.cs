namespace DataJam.TestSupport.FluentMigrator.Core;

using JetBrains.Annotations;

[PublicAPI]
public interface IDescribeSchemas
{
    public string SchemaName { get; }
}
