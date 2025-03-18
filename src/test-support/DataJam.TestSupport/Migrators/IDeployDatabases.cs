namespace DataJam.TestSupport;

using System.Threading.Tasks;

using JetBrains.Annotations;

[PublicAPI]
public interface IDeployDatabases
{
    Task Deploy();
}
