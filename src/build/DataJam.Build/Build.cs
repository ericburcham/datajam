// ReSharper disable InconsistentNaming
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

    /// <summary>The API key for publishing nuGet packages to nuGet.org.</summary>
    [Parameter("NuGet.org - NuGet target API key / access token", Name = "NuGetOrgTargetApiKey")]
    private readonly string? _nuGetOrgTargetApiKey;

    /// <summary>The URl for publishing nuGet packages to nuGet.org.</summary>
    [Parameter("NuGet.org - NuGet target URL", Name = "NuGetOrgTargetUrl")]
    private readonly string? _nuGetOrgTargetUrl;

    /// <summary>The API key for publishing nuGet packages to JetBrains space.</summary>
    [Parameter("Space - NuGet target API key / access token", Name = "NuGetSpaceTargetApiKey")]
    private readonly string? _nuGetSpaceTargetApiKey;

    /// <summary>The URL for publishing nuGet packages to JetBrains space.</summary>
    [Parameter("Space - NuGet target URL", Name = "NuGetSpaceTargetUrl")]
    private readonly string? _nuGetSpaceTargetUrl;

    /// <summary>Gets information about the git repository.</summary>
    [GitRepository]
    private readonly GitRepository _repository = null!;

    /// <summary>Gets version information.</summary>
    [VersionInfo]
    private readonly VersionInfo? _versionInfo;

    /// <summary>Gets the DotNet solution.</summary>
    [Solution]
    private readonly Solution Solution = null!;

    /// <summary>Gets the absolute path for the project's source code.</summary>
    private static AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    /// <summary>Gets the absolute path for the project's test folder.</summary>
    private static AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    /// <summary>Gets the absolute path for nuGet package output.</summary>
    private AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    /// <summary>Gets a target that cleans artifact, bin, and obj folders.</summary>
    private Target Clean =>
        _ => _
           .Executes(
                () =>
                {
                    SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                    TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                    ArtifactsDirectory.CreateOrCleanDirectory();
                });

    /// <summary>Gets a target that runs DotNet Build for the solution.</summary>
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

    /// <summary>Gets a target that runs DotNet Pack for the solution.</summary>
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

    // ReSharper disable once UnusedMember.Local
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

    // ReSharper disable once UnusedMember.Local
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

    /// <summary>Gets a target that restores package dependencies.</summary>
    private Target Restore =>
        _ => _
            .DependsOn(Clean)
            .Executes(
                 () =>
                 {
                     DotNetRestore(_ => _.SetProjectFile(Solution));
                 });

    /// <summary>Gets a target that runs DotNet Test for the solution.</summary>
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

    // Support plugins are available for:
    // - JetBrains ReSharper        https://nuke.build/resharper
    // - JetBrains Rider            https://nuke.build/rider
    // - Microsoft VisualStudio     https://nuke.build/visualstudio
    // - Microsoft VSCode           https://nuke.build/vscode
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
