// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable VariableHidesOuterVariable

namespace DataJam.Build;

using System;
using System.Linq;

using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
public class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)", Name = "Configuration")]
    private readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("NuGet.org - NuGet target API key / access token", Name = "NuGetOrgTargetApiKey")]
    private readonly string? _nuGetOrgTargetApiKey;

    [Parameter("NuGet.org - NuGet target URL", Name = "NuGetOrgTargetUrl")]
    private readonly string? _nuGetOrgTargetUrl;

    [Parameter("Space - NuGet target API key / access token", Name = "NuGetSpaceTargetApiKey")]
    private readonly string? _nuGetSpaceTargetApiKey;

    [Parameter("Space - NuGet target URL", Name = "NuGetSpaceTargetUrl")]
    private readonly string? _nuGetSpaceTargetUrl;

    [GitRepository]
    private readonly GitRepository _repository = null!;

    [VersionInfo]
    private readonly VersionInfo? _versionInfo;

    [Solution]
    private readonly Solution Solution = null!;

    private static AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    private static AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    private AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    private Target Clean =>
        _ => _
           .Executes(
                () =>
                {
                    SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                    TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                    ArtifactsDirectory.CreateOrCleanDirectory();
                });

    private Target Compile =>
        _ => _
            .DependsOn(Restore)
            .Executes(
                 () =>
                 {
                     DotNetBuild(
                         _ => _
                             .EnableNoRestore()
                             .SetAssemblyVersion(_versionInfo?.AssemblyVersion)
                             .SetConfiguration(_configuration)
                             .SetFileVersion(_versionInfo?.FileVersion)
                             .SetInformationalVersion(_versionInfo?.InformationalVersion)
                             .SetProjectFile(Solution)
                             .SetProperty("GeneratePackageOnBuild", "False")
                             .SetVersionPrefix(_versionInfo?.VersionPrefix)
                             .SetVersionSuffix(_versionInfo?.VersionSuffix)
                             .SetVersion(_versionInfo?.Version));
                 });

    private Target Package =>
        _ => _
            .DependsOn(Test)
            .Executes(
                 () =>
                 {
                     foreach (var project in Solution.AllProjects.Where(p => p.GetProperty<bool>("GeneratePackageOnBuild")))
                     {
                         DotNetPack(
                             _ => _
                                 .EnableIncludeSource()
                                 .EnableIncludeSymbols()
                                 .EnableNoBuild()
                                 .EnableNoRestore()
                                 .SetConfiguration(_configuration)
                                 .SetProject(project)
                                 .SetOutputDirectory(ArtifactsDirectory)
                                 .SetVersion(_versionInfo?.PackageVersion));
                     }
                 });

    private Target PublishPackagesToSpace =>
        _ => _
            .TriggeredBy(Package)
            .OnlyWhenStatic(ShouldPublishToSpace())
            .WhenSkipped(DependencyBehavior.Execute)
            .Executes(
                 () =>
                 {
                     PublishPackages(_nuGetSpaceTargetApiKey, _nuGetSpaceTargetUrl);
                 });

    private Target PushPackagesToNuGetOrg =>
        _ => _
            .TriggeredBy(Package)
            .OnlyWhenStatic(ShouldPublishToNuGetOrg())
            .WhenSkipped(DependencyBehavior.Execute)
            .Executes(
                 () =>
                 {
                     PublishPackages(_nuGetOrgTargetApiKey, _nuGetOrgTargetUrl);
                 });

    private Target Restore =>
        _ => _
            .DependsOn(Clean)
            .Executes(
                 () =>
                 {
                     DotNetRestore(_ => _.SetProjectFile(Solution));
                 });

    private Target Test =>
        _ => _
            .DependsOn(Compile)
            .Executes(
                 () =>
                 {
                     DotNetTest(
                         _ => _
                             .EnableNoBuild()
                             .EnableNoRestore()
                             .SetConfiguration(_configuration)
                             .SetProjectFile(Solution));
                 });

    public static int Main() => Execute<Build>(build => build.Package);

    private void PublishPackages(string? apiKey, string? targetUrl)
    {
        var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

        DotNetNuGetPush(
            _ => _
                .EnableSkipDuplicate()
                .SetApiKey(apiKey)
                .SetSource(targetUrl)
                .CombineWith(packages, (settings, path) => settings.SetTargetPath(path)),
            5,
            true);
    }

    private Func<bool> ShouldPublishToNuGetOrg() =>
        () => _repository.IsOnMainBranch()
              && (Environment.GetEnvironmentVariable("JB_SPACE_GIT_BRANCH") ?? string.Empty).Contains("main", StringComparison.OrdinalIgnoreCase)
              && !string.IsNullOrEmpty(_nuGetOrgTargetApiKey)
              && !string.IsNullOrEmpty(_nuGetOrgTargetUrl)
              && _nuGetOrgTargetUrl != " ";

    private Func<bool> ShouldPublishToSpace() => () => !string.IsNullOrEmpty(_nuGetSpaceTargetApiKey) && !string.IsNullOrEmpty(_nuGetSpaceTargetUrl) && _nuGetSpaceTargetUrl != " ";
}
