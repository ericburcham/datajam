// ReSharper disable InconsistentNaming

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
    private readonly GitRepository GitRepository = null!;

    /// <summary>Gets the DotNet solution.</summary>
    [Solution]
    private readonly Solution Solution = null!;

    /// <summary>Gets version information.</summary>
    [VersionInfo]
    private readonly VersionInfo? VersionInfo;

    /// <summary>Gets the absolute path for the project's source code.</summary>
    private static AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    /// <summary>Gets the absolute path for the project's test folder.</summary>
    private static AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    /// <summary>Gets the absolute path for nuGet package output.</summary>
    private AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    /// <summary>Gets a target that cleans artifact, bin, and obj folders.</summary>
    private Target Clean =>
        _ => _.Executes(
            () =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                ArtifactsDirectory.CreateOrCleanDirectory();
            });

    /// <summary>Gets a target that runs DotNet Build for the solution.</summary>
    private Target Compile =>
        _ => _.DependsOn(Restore)
              .Executes(
                   () =>
                   {
                       DotNetBuild(
                           buildSettings =>
                               buildSettings
                                  .EnableNoRestore()
                                  .SetAssemblyVersion(VersionInfo?.AssemblyVersion)
                                  .SetConfiguration(_configuration)
                                  .SetFileVersion(VersionInfo?.FileVersion)
                                  .SetInformationalVersion(VersionInfo?.InformationalVersion)
                                  .SetProjectFile(Solution)
                                  .SetProperty("GeneratePackageOnBuild", "False")
                                  .SetVersionPrefix(VersionInfo?.VersionPrefix)
                                  .SetVersionSuffix(VersionInfo?.VersionSuffix)
                                  .SetVersion(VersionInfo?.Version));
                   });

    /// <summary>Gets a target that runs DotNet Pack for the solution.</summary>
    private Target Package =>
        _ => _.DependsOn(Compile)
              .Executes(
                   () =>
                   {
                       foreach (var project in Solution.AllProjects.Where(p => p.GetProperty<bool>("GeneratePackageOnBuild")))
                       {
                           DotNetPack(
                               packSettings => packSettings
                                              .EnableIncludeSource()
                                              .EnableIncludeSymbols()
                                              .EnableNoBuild()
                                              .EnableNoRestore()
                                              .SetConfiguration(_configuration)
                                              .SetProject(project)
                                              .SetOutputDirectory(ArtifactsDirectory)
                                              .SetVersion(VersionInfo?.PackageVersion));
                       }
                   });

    // ReSharper disable once UnusedMember.Local
    private Target PublishPackagesToSpace =>
        _ => _.TriggeredBy(Package)
              .OnlyWhenStatic(() => !string.IsNullOrEmpty(_nuGetSpaceTargetApiKey) && !string.IsNullOrEmpty(_nuGetSpaceTargetUrl) && _nuGetSpaceTargetUrl != " ")
              .WhenSkipped(DependencyBehavior.Execute)
              .Executes(
                   () =>
                   {
                       Console.WriteLine("--------------------------------------------------------------------------------");
                       Console.WriteLine("Attempting to publish NuGet packages to Space.  Diagnostic information follows.");
                       Console.WriteLine($"API Key   : {_nuGetSpaceTargetApiKey}");
                       Console.WriteLine($"Target URL: {_nuGetSpaceTargetUrl}");
                       Console.WriteLine("--------------------------------------------------------------------------------");

                       var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

                       DotNetNuGetPush(
                           pushSettings => pushSettings.EnableSkipDuplicate()
                                                       .SetApiKey(_nuGetSpaceTargetApiKey)
                                                       .SetSource(_nuGetSpaceTargetUrl)
                                                       .CombineWith(packages, (combinedPushSettings, targetPath) => combinedPushSettings.SetTargetPath(targetPath)),
                           5,
                           true);
                   });

    // ReSharper disable once UnusedMember.Local
    private Target PushPackagesToNuGetOrg =>
        _ => _.TriggeredBy(Package)
              .OnlyWhenStatic(
                   () => GitRepository.IsOnMainBranch()
                         && (Environment.GetEnvironmentVariable("JB_SPACE_GIT_BRANCH") ?? string.Empty).Contains("main", StringComparison.OrdinalIgnoreCase)
                         && !string.IsNullOrEmpty(_nuGetOrgTargetApiKey)
                         && !string.IsNullOrEmpty(_nuGetOrgTargetUrl)
                         && _nuGetOrgTargetUrl != " ")
              .WhenSkipped(DependencyBehavior.Execute)
              .Executes(
                   () =>
                   {
                       var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

                       DotNetNuGetPush(
                           pushSettings => pushSettings.SetApiKey(_nuGetOrgTargetApiKey)
                                                       .SetSource(_nuGetOrgTargetUrl)
                                                       .CombineWith(packages, (combinedPushSettings, targetPath) => combinedPushSettings.SetTargetPath(targetPath)),
                           5,
                           true);
                   });

    /// <summary>Gets a target that restores package dependencies.</summary>
    private Target Restore =>
        _ => _.DependsOn(Clean)
              .Executes(
                   () =>
                   {
                       DotNetRestore(restoreSettings => restoreSettings.SetProjectFile(Solution));
                   });

    /// <summary>Gets a target that runs DotNet Test for the solution.</summary>
    private Target Test =>
        _ => _.DependsOn(Compile)
              .Executes(
                   () =>
                   {
                       DotNetTest(testSettings => testSettings.EnableNoBuild().EnableNoRestore().SetConfiguration(_configuration).SetProjectFile(Solution));
                   });

    // Support plugins are available for:
    // - JetBrains ReSharper        https://nuke.build/resharper
    // - JetBrains Rider            https://nuke.build/rider
    // - Microsoft VisualStudio     https://nuke.build/visualstudio
    // - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(build => build.Package);
}
