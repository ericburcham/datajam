namespace DataJam.TestSupport.Dependencies.TestContainers;

using JetBrains.Annotations;

using Testcontainers.MySql;

/// <summary>Provides a default builder for MySQL test containers.</summary>
/// <remarks>
///     The default <see cref="MySqlBuilder" /> instance provided by the TestContainers.MySql package does not result in a connection string that can be used
///     successfully.  To work around this, we currently require the call site to provide a username and password which are used to configure the container.  These
///     credentials are included when invoking the container's <see cref="MySqlContainer.GetConnectionString" /> method.  If you wish to use another authentication
///     method, make your own derivative of <see cref="TestDependencyBuilder{MySqlContainer}" /> and use it instead of
///     <see cref="DefaultMySqlTestContainerBuilder" />.
/// </remarks>
/// <param name="userName">The username to use.</param>
/// <param name="password">The password to use.</param>
[PublicAPI]
public class DefaultMySqlTestContainerBuilder(string userName, string password) : ContainerBuilder<MySqlContainer>
{
    protected override MySqlContainer BuildContainer()
    {
        return new MySqlBuilder()
              .WithPassword(password)
              .WithUsername(userName)
              .Build();
    }
}
