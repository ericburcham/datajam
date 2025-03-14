namespace DataJam.TestSupport.FluentMigrator;

using JetBrains.Annotations;

[PublicAPI]
public interface IProvideKeyValues<out TValue>
{
    public TValue KeyValue { get; }
}
