namespace DataJam.TestSupport.TestContainers;

using Testcontainers.MySql;

public class DefaultMySqlTestContainerBuilder : TestContainerBuilder<MySqlContainer>
{
    public override MySqlContainer Build()
    {
        return new MySqlBuilder().Build();
    }
}
