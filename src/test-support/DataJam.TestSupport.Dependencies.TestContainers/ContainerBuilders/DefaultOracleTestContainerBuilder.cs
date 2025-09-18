namespace DataJam.TestSupport.Dependencies.TestContainers;

using JetBrains.Annotations;

using Testcontainers.Oracle;

/// <summary>Provides a default builder for Oracle test containers.</summary>
/// <remarks>
///     The default <see cref="OracleBuilder" /> instance provided by the TestContainers.Oracle package provides a working Oracle container with default
///     credentials. The container includes a default database instance with system users. For custom authentication or configuration, create your own derivative
///     of <see cref="TestDependencyBuilder{OracleContainer}" /> instead of <see cref="DefaultOracleTestContainerBuilder" />.
/// </remarks>
/// <param name="password">The password to use for the Oracle container.</param>
[PublicAPI]
public class DefaultOracleTestContainerBuilder(string password) : ContainerBuilder<OracleContainer>
{
    protected override OracleContainer BuildContainer()
    {
        return new OracleBuilder()
              .WithPassword(password)
              .Build();
    }
}
