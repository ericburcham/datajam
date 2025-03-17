namespace DataJam.TestSupport.Dependencies.TestContainers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

using Microsoft.Extensions.Logging;

using Testcontainers.MsSql;

public class DefaultMsSqlTestContainerBuilder : ContainerBuilder<MsSqlContainer>
{
    protected override MsSqlContainer BuildContainer()
    {
        return new MsSqlBuilder().Build();
    }
}

public abstract class ContainerBuilder<T> : TestDependencyBuilder<ContainerAdapter<T>>
    where T : IContainer
{
    public override ContainerAdapter<T> Build()
    {
        var container = BuildContainer();

        return new(container);
    }

    protected abstract T BuildContainer();
}

public class ContainerAdapter<T> : IAsyncStartableTestDependency<T>, IContainer
    where T : IContainer
{
    public ContainerAdapter(T container)
    {
        Dependency = container;
    }

    public event EventHandler? Created
    {
        add => Dependency.Created += value;

        remove => Dependency.Created -= value;
    }

    public event EventHandler? Creating
    {
        add => Dependency.Creating += value;

        remove => Dependency.Creating -= value;
    }

    public event EventHandler? Started
    {
        add => Dependency.Started += value;

        remove => Dependency.Started -= value;
    }

    public event EventHandler? Starting
    {
        add => Dependency.Starting += value;

        remove => Dependency.Starting -= value;
    }

    public event EventHandler? Stopped
    {
        add => Dependency.Stopped += value;

        remove => Dependency.Stopped -= value;
    }

    public event EventHandler? Stopping
    {
        add => Dependency.Stopping += value;

        remove => Dependency.Stopping -= value;
    }

    public DateTime CreatedTime => Dependency.CreatedTime;

    public T Dependency { get; }

    public TestcontainersHealthStatus Health => Dependency.Health;

    public long HealthCheckFailingStreak => Dependency.HealthCheckFailingStreak;

    public string Hostname => Dependency.Hostname;

    public string Id => Dependency.Id;

    public IImage Image => Dependency.Image;

    public string IpAddress => Dependency.IpAddress;

    public ILogger Logger => Dependency.Logger;

    public string MacAddress => Dependency.MacAddress;

    public string Name => Dependency.Name;

    public DateTime StartedTime => Dependency.StartedTime;

    public TestcontainersStates State => Dependency.State;

    public DateTime StoppedTime => Dependency.StoppedTime;

    object ITestDependency.Dependency => Dependency;

    public Task CopyAsync(
        byte[] fileContent,
        string filePath,
        UnixFileModes fileMode = UnixFileModes.None | UnixFileModes.OtherRead | UnixFileModes.GroupRead | UnixFileModes.UserWrite | UnixFileModes.UserRead,
        CancellationToken ct = default)
    {
        return Dependency.CopyAsync(fileContent, filePath, fileMode, ct);
    }

    public Task CopyAsync(
        string source,
        string target,
        UnixFileModes fileMode = UnixFileModes.None | UnixFileModes.OtherRead | UnixFileModes.GroupRead | UnixFileModes.UserWrite | UnixFileModes.UserRead,
        CancellationToken ct = default)
    {
        return Dependency.CopyAsync(source, target, fileMode, ct);
    }

    public Task CopyAsync(
        DirectoryInfo source,
        string target,
        UnixFileModes fileMode = UnixFileModes.None | UnixFileModes.OtherRead | UnixFileModes.GroupRead | UnixFileModes.UserWrite | UnixFileModes.UserRead,
        CancellationToken ct = default)
    {
        return Dependency.CopyAsync(source, target, fileMode, ct);
    }

    public Task CopyAsync(
        FileInfo source,
        string target,
        UnixFileModes fileMode = UnixFileModes.None | UnixFileModes.OtherRead | UnixFileModes.GroupRead | UnixFileModes.UserWrite | UnixFileModes.UserRead,
        CancellationToken ct = default)
    {
        return Dependency.CopyAsync(source, target, fileMode, ct);
    }

    public ValueTask DisposeAsync()
    {
        return Dependency.DisposeAsync();
    }

    public Task<ExecResult> ExecAsync(IList<string> command, CancellationToken ct = default)
    {
        return Dependency.ExecAsync(command, ct);
    }

    public Task<long> GetExitCodeAsync(CancellationToken ct = default)
    {
        return Dependency.GetExitCodeAsync(ct);
    }

    public Task<(string Stdout, string Stderr)> GetLogsAsync(DateTime since = default, DateTime until = default, bool timestampsEnabled = true, CancellationToken ct = default)
    {
        return Dependency.GetLogsAsync(since, until, timestampsEnabled, ct);
    }

    public ushort GetMappedPublicPort(int containerPort)
    {
        return Dependency.GetMappedPublicPort(containerPort);
    }

    public ushort GetMappedPublicPort(string containerPort)
    {
        return Dependency.GetMappedPublicPort(containerPort);
    }

    public Task<byte[]> ReadFileAsync(string filePath, CancellationToken ct = default)
    {
        return Dependency.ReadFileAsync(filePath, ct);
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
