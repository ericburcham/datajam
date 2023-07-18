using System.Linq;

using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable InconsistentNaming
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)", Name = "Configuration")]
    readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    /// <summary>The API key for publishing nuget packages to nuget.org.</summary>
    [Parameter("NuGet.org - NuGet target API key / access token", Name = "NuGetOrgTargetApiKey")]
    readonly string? _nugetOrgTargetApiKey;

    /// <summary>The URl for publishing nuget packages to nuget.org.</summary>
    [Parameter("NuGet.org - NuGet target URL", Name = "NuGetOrgTargetUrl")]
    readonly string? _nugetOrgTargetUrl;

    /// <summary>The API key for publishing nuget packages to JetBrains space.</summary>
    [Parameter("Space - NuGet target API key / access token", Name = "NuGetPublicSpaceTargetApiKey")]
    readonly string? _nugetPublicSpaceTargetApiKey;

    /// <summary>The URL for publishing nuget packages to JetBrains space.</summary>
    [Parameter("Space - NuGet target URL", Name = "NuGetPublicSpaceTargetUrl")]
    readonly string? _nugetPublicSpaceTargetUrl;

    /// <summary>Gets the DotNet solution.</summary>
    [Solution]
    readonly Solution Solution = null!;

    /// <summary>Gets the absolute path for the project's source code.</summary>
    static AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    /// <summary>Gets the absolute path for the project's test folder.</summary>
    static AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    /// <summary>Gets the absolute path for nuget package output.</summary>
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
