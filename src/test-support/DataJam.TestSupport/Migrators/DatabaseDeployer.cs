namespace DataJam.TestSupport;

using System.Reflection;
using System.Threading.Tasks;

public abstract class DatabaseDeployer : IDeployDatabases
{
    protected abstract Assembly MigrationAssembly { get; }

    public Task Deploy()
    {
        return DeployInternal(MigrationAssembly);
    }

    protected abstract Task DeployInternal(Assembly migrationAssembly);
}
