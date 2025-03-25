namespace DataJam.EntityFramework;

using System.Data.Common;

using JetBrains.Annotations;

/// <summary>Provides an adapter from <see cref="DbConnectionStringBuilder" /> instances to the <see cref="IProvideNameOrConnectionString" /> interface.</summary>
/// <param name="connectionStringBuilder">The connection string builder to use.</param>
[PublicAPI]
public class DbConnectionStringBuilderAdapter(DbConnectionStringBuilder connectionStringBuilder) : IProvideNameOrConnectionString
{
    /// <inheritdoc cref="IProvideNameOrConnectionString.NameOrConnectionString" />
    public string NameOrConnectionString { get; } = connectionStringBuilder.ConnectionString;
}
