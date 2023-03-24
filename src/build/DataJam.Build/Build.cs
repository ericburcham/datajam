using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

/// <summary>Exposes and coordinates <see cref="Target" />s for use by Nuke Build.</summary>
[UnsetVisualStudioEnvironmentVariables]
public class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    private readonly Solution _solution = null!;

    private AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    private Target Clean =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                EnsureCleanDirectory(ArtifactsDirectory);
            });

    private Target Compile =>
        targetDefinition => targetDefinition.DependsOn(Restore)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetBuild(buildSettings => buildSettings.SetProjectFile(_solution).SetConfiguration(_configuration).EnableNoRestore());
                                                 });

    private Target Pack =>
        targetDefinition => targetDefinition.DependsOn(Test)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetPack(packSettings => packSettings.SetProject(_solution).SetOutputDirectory(ArtifactsDirectory).SetIncludeSymbols(true).SetConfiguration(_configuration).EnableNoRestore().EnableNoBuild());
                                                 });

    private Target Restore =>
        targetDefinition => targetDefinition.DependsOn(Clean)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetRestore(restoreSettings => restoreSettings.SetProjectFile(_solution));
                                                 });

    private AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    private Target Test =>
        targetDefinition => targetDefinition.DependsOn(Compile)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetTest(testSettings => testSettings.SetProjectFile(_solution).SetConfiguration(_configuration).EnableNoRestore().EnableNoBuild());
                                                 });

    private AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    // Support plugins are available for:
    // - JetBrains ReSharper        https://nuke.build/resharper
    // - JetBrains Rider            https://nuke.build/rider
    // - Microsoft VisualStudio     https://nuke.build/visualstudio
    // - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(build => build.Pack);
}
