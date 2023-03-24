using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    #pragma warning disable SA1306 // Field names should begin with lower-case letter
    #pragma warning disable SX1309 // Field names should begin with underscore
    [Solution]
    readonly Solution Solution = null!;
    #pragma warning restore SX1309 // Field names should begin with underscore
    #pragma warning restore SA1306 // Field names should begin with lower-case letter

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                EnsureCleanDirectory(ArtifactsDirectory);
            });

    Target Compile =>
        targetDefinition => targetDefinition.DependsOn(Restore)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetBuild(buildSettings => buildSettings.SetProjectFile(Solution).SetConfiguration(_configuration).EnableNoRestore());
                                                 });

    Target Pack =>
        targetDefinition => targetDefinition.DependsOn(Test)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetPack(packSettings => packSettings.SetProject(Solution).SetOutputDirectory(ArtifactsDirectory).SetIncludeSymbols(true).SetConfiguration(_configuration).EnableNoRestore().EnableNoBuild());
                                                 });

    Target Restore =>
        targetDefinition => targetDefinition.DependsOn(Clean)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetRestore(restoreSettings => restoreSettings.SetProjectFile(Solution));
                                                 });

    AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    Target Test =>
        targetDefinition => targetDefinition.DependsOn(Compile)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetTest(testSettings => testSettings.SetProjectFile(Solution).SetConfiguration(_configuration).EnableNoRestore().EnableNoBuild());
                                                 });

    AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    // Support plugins are available for:
    // - JetBrains ReSharper        https://nuke.build/resharper
    // - JetBrains Rider            https://nuke.build/rider
    // - Microsoft VisualStudio     https://nuke.build/visualstudio
    // - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(build => build.Pack);
}
