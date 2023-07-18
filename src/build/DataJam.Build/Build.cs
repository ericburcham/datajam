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

// ReSharper disable InconsistentNaming
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)", Name = "Configuration")]
    readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    /// <summary>The API key for publishing nuGet packages to nuGet.org.</summary>
    [Parameter("NuGet.org - NuGet target API key / access token", Name = "NuGetOrgTargetApiKey")]
    readonly string? _nuGetOrgTargetApiKey;

    /// <summary>The URl for publishing nuGet packages to nuGet.org.</summary>
    [Parameter("NuGet.org - NuGet target URL", Name = "NuGetOrgTargetUrl")]
    readonly string? _nuGetOrgTargetUrl;

    /// <summary>The API key for publishing nuGet packages to JetBrains space.</summary>
    [Parameter("Space - NuGet target API key / access token", Name = "NuGetSpaceTargetApiKey")]
    readonly string? _nuGetSpaceTargetApiKey;

    /// <summary>The URL for publishing nuGet packages to JetBrains space.</summary>
    [Parameter("Space - NuGet target URL", Name = "NuGetSpaceTargetUrl")]
    readonly string? _nuGetSpaceTargetUrl;

    /// <summary>Gets information about the git repository.</summary>
    [GitRepository]
    readonly GitRepository Repository = null!;

    /// <summary>Gets the DotNet solution.</summary>
    [Solution]
    readonly Solution Solution = null!;

    /// <summary>Gets the absolute path for the project's source code.</summary>
    static AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    /// <summary>Gets the absolute path for the project's test folder.</summary>
    static AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    /// <summary>Gets the absolute path for nuGet package output.</summary>
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    /// <summary>Gets a target that cleans artifact, bin, and obj folders.</summary>
    Target Clean =>
        _ => _.Executes(
            () =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                ArtifactsDirectory.CreateOrCleanDirectory();
            });

    /// <summary>Gets a target that runs DotNet Build for the solution.</summary>
    Target Compile =>
        _ => _.DependsOn(Restore)
              .Executes(
                   () =>
                   {
                       DotNetBuild(buildSettings => buildSettings.EnableNoRestore().SetConfiguration(_configuration).SetProjectFile(Solution).SetProperty("GeneratePackageOnBuild", "false"));
                   });

    /// <summary>Gets a target that runs DotNet Pack for the solution.</summary>
    Target Package =>
        _ => _.DependsOn(Test)
              .Executes(
                   () =>
                   {
                       foreach (var project in Solution.AllProjects.Where(p => p.GetProperty<bool>("GeneratePackageOnBuild")))
                       {
                           DotNetPack(packSettings => packSettings.EnableIncludeSource().EnableIncludeSymbols().EnableNoBuild().EnableNoRestore().SetConfiguration(_configuration).SetProject(project).SetOutputDirectory(ArtifactsDirectory));
                       }
                   });

    // ReSharper disable once UnusedMember.Local
    Target PublishPackagesToSpace =>
        _ => _.TriggeredBy(Package)
              .OnlyWhenStatic(() => !string.IsNullOrEmpty(_nuGetSpaceTargetApiKey) && !string.IsNullOrEmpty(_nuGetSpaceTargetUrl) && _nuGetSpaceTargetUrl != " ")
              .WhenSkipped(DependencyBehavior.Execute)
              .Executes(
                   () =>
                   {
                       var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

                       DotNetNuGetPush(pushSettings => pushSettings.SetApiKey(_nuGetSpaceTargetApiKey).SetSource(_nuGetSpaceTargetUrl).CombineWith(packages, (combinedPushSettings, targetPath) => combinedPushSettings.SetTargetPath(targetPath)), 5, true);
                   });

    // ReSharper disable once UnusedMember.Local
    Target PushPackagesToNuGetOrg =>
        _ => _.TriggeredBy(Package)
              .OnlyWhenStatic(() => Repository.IsOnMainBranch() && (Environment.GetEnvironmentVariable("JB_SPACE_GIT_BRANCH") ?? string.Empty).Contains("main", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(_nuGetOrgTargetApiKey) && !string.IsNullOrEmpty(_nuGetOrgTargetUrl) && _nuGetOrgTargetUrl != " ")
              .WhenSkipped(DependencyBehavior.Execute)
              .Executes(
                   () =>
                   {
                       var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

                       DotNetNuGetPush(pushSettings => pushSettings.SetApiKey(_nuGetOrgTargetApiKey).SetSource(_nuGetOrgTargetUrl).CombineWith(packages, (combinedPushSettings, targetPath) => combinedPushSettings.SetTargetPath(targetPath)), 5, true);
                   });

    /// <summary>Gets a target that restores package dependencies.</summary>
    Target Restore =>
        _ => _.DependsOn(Clean)
              .Executes(
                   () =>
                   {
                       DotNetRestore(restoreSettings => restoreSettings.SetProjectFile(Solution));
                   });

    /// <summary>Gets a target that runs DotNet Test for the solution.</summary>
    Target Test =>
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
