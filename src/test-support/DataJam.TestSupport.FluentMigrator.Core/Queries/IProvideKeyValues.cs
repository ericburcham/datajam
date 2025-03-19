namespace DataJam.TestSupport.FluentMigrator.Core;

using JetBrains.Annotations;

[PublicAPI]
public interface IProvideKeyValues<out TValue>
{
    public TValue KeyValue { get; }
}
