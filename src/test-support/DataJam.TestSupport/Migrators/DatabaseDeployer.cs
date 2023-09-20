namespace DataJam.TestSupport.Migrators;

using System.Reflection;
using System.Threading.Tasks;

public abstract class DatabaseDeployer : IDeployDatabases
{
    private readonly string _migrationAssembly;

    protected DatabaseDeployer(string migrationAssembly)
    {
        _migrationAssembly = migrationAssembly;
    }

    public Task Deploy()
    {
        var migrationAssembly = Assembly.Load(_migrationAssembly);

        return DeployInternal(migrationAssembly);
    }

    protected abstract Task DeployInternal(Assembly migrationAssembly);
}
