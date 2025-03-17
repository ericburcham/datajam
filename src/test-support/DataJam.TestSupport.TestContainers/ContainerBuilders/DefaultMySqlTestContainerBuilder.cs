namespace DataJam.TestSupport.TestContainers;

using Testcontainers.MySql;

public class DefaultMySqlTestContainerBuilder : TestContainerBuilder<MySqlContainer>
{
    private readonly string _password;

    private readonly string _userName;

    /// <summary>Initializes a new instance of the <see cref="DefaultMySqlTestContainerBuilder" /> class.</summary>
    /// <remarks>
    ///     The default <see cref="MySqlBuilder" /> instance provided by the TestContainers.MySql package does not result in a connection string that can be used
    ///     successfully.  To work around this, we currently require the call site to provide a username and password which are used to configure the container.  These
    ///     credentials are included when invoking the container's <see cref="MySqlContainer.GetConnectionString" /> method.  If you wish to use another authentication
    ///     method, make your own derivative of <see cref="TestContainerBuilder{MySqlContainer}" /> and use it instead of
    ///     <see cref="DefaultMySqlTestContainerBuilder" />.
    /// </remarks>
    /// <param name="userName">The username to use.</param>
    /// <param name="password">The password to use.</param>
    public DefaultMySqlTestContainerBuilder(string userName, string password)
    {
        _userName = userName;
        _password = password;
    }

    public override MySqlContainer Build()
    {
        return new MySqlBuilder()
              .WithPassword(_password)
              .WithUsername(_userName)
              .Build();
    }
}
