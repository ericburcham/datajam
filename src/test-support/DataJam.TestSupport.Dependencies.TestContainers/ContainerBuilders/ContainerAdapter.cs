namespace DataJam.TestSupport.Dependencies.TestContainers;

using System;
using System.Threading;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using JetBrains.Annotations;

[PublicAPI]
public class ContainerAdapter<T>(T container) : TestDependency<T>(container), IAsyncStartableTestDependency, IAsyncDisposable
    where T : class, IContainer
{
    public async ValueTask DisposeAsync()
    {
        await Dependency.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(CancellationToken ct = default)
    {
        return Dependency.StartAsync(ct);
    }

    public Task StopAsync(CancellationToken ct = default)
    {
        return Dependency.StopAsync(ct);
    }
}
