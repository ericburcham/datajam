using System.Linq;

using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using Serilog;

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

    /// <summary>Gets the absolute path for nuget package output.</summary>
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    /// <summary>Gets a target that cleans artifact, bin, and obj folders.</summary>
    Target Clean =>
        _ => _.Executes(
            () =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(x => x.DeleteDirectory());
                ArtifactsDirectory.CreateDirectory();
            });

    /// <summary>Gets a target that runs DotNet Build for the solution.</summary>
    Target Compile =>
        _ => _.DependsOn(Restore)
              .Executes(
                   () =>
                   {
                       DotNetBuild(_ => _.EnableNoRestore().SetConfiguration(_configuration).SetProjectFile(Solution).SetProperty("GeneratePackageOnBuild", "False"));
                   });

    /// <summary>Gets a target that runs the default build.</summary>
    Target Default => _ => _.DependsOn(Clean, Publish);

    /// <summary>Gets a target that runs DotNet Pack for the solution.</summary>
    Target Package =>
        _ => _.DependsOn(Test)
              .Executes(
                   () =>
                   {
                       foreach (var project in Solution.AllProjects.Where(p => p.GetProperty<bool>("GeneratePackageOnBuild")))
                       {
                           DotNetPack(_ => _.EnableIncludeSource().EnableIncludeSymbols().EnableNoBuild().EnableNoRestore().SetConfiguration(_configuration).SetProject(project).SetOutputDirectory(ArtifactsDirectory));
                       }
                   });

    Target Publish =>
        _ => _.DependsOn(Package)
              .OnlyWhenStatic(() => !Repository.IsOnMainOrMasterBranch())
              .Executes(
                   () =>
                   {
                       // Parameters and secrets:  https://www.jetbrains.com/help/space/automation-parameters.html#pass-parameters-as-environment-variables
                       // .NET and .NET Core:      https://www.jetbrains.com/help/space/net-core.html
                       Log.Information("Commit = {Value}", Repository.Commit);
                       Log.Information("Branch = {Value}", Repository.Branch);
                       Log.Information("Tags = {Value}", Repository.Tags);

                       Log.Information("main branch = {Value}", Repository.IsOnMainBranch());
                       Log.Information("main/master branch = {Value}", Repository.IsOnMainOrMasterBranch());
                       Log.Information("develop branch = {Value}", Repository.IsOnDevelopBranch());
                       Log.Information("release/* branch = {Value}", Repository.IsOnReleaseBranch());
                       Log.Information("hotfix/* branch = {Value}", Repository.IsOnHotfixBranch());

                       Log.Information("Https URL = {Value}", Repository.HttpsUrl);
                       Log.Information("SSH URL = {Value}", Repository.SshUrl);
                   });

    /// <summary>Gets a target that restores package dependencies.</summary>
    Target Restore =>
        _ => _.DependsOn(Clean)
              .Executes(
                   () =>
                   {
                       DotNetRestore(_ => _.SetProjectFile(Solution));
                   });

    /// <summary>Gets a target that runs DotNet Test for the solution.</summary>
    Target Test =>
        _ => _.DependsOn(Compile)
              .Executes(
                   () =>
                   {
                       DotNetTest(_ => _.EnableNoBuild().EnableNoRestore().SetConfiguration(_configuration).SetProjectFile(Solution));
                   });

    // Support plugins are available for:
    // - JetBrains ReSharper        https://nuke.build/resharper
    // - JetBrains Rider            https://nuke.build/rider
    // - Microsoft VisualStudio     https://nuke.build/visualstudio
    // - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(build => build.Default);
}
