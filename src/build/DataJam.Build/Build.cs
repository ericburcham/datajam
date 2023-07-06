using System;

using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable InconsistentNaming
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    readonly Solution Solution = null!;

    /// <summary>Gets a target that runs dotnet clean for all configurations, removes all bin and object folders, and ensures the package output folder is clean.</summary>
    Target CleanAll => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, CleanOutput, CleanPackages);

    /// <summary>Gets a target that runs dotnet clean for the debug configuration.</summary>
    Target CleanDebug => targetDefinition => targetDefinition.Executes(DoStandardClean(Configuration.Debug));

    /// <summary>Gets a target that removes all bin and object folders.</summary>
    Target CleanOutput =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            });

    /// <summary>Gets a target that ensures the package output folder is clean.</summary>
    Target CleanPackages =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                EnsureCleanDirectory(PackagesDirectory);
            });

    /// <summary>Gets a target that runs dotnet clean for the release configuration.</summary>
    Target CleanRelease => targetDefinition => targetDefinition.Executes(DoStandardClean(Configuration.Release));

    Target Compile =>
        targetDefinition => targetDefinition.DependsOn(Restore)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetBuild(buildSettings => buildSettings.SetProjectFile(Solution).SetConfiguration(Configuration).EnableNoRestore());
                                                 });

    Target CompileDebug =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                DotNetBuild(buildSettings => buildSettings.SetProjectFile(Solution).SetConfiguration(Configuration.Debug).EnableNoRestore());
            });

    Target CompileRelease =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                DotNetBuild(buildSettings => buildSettings.SetProjectFile(Solution).SetConfiguration(Configuration.Release).EnableNoRestore());
            });

    Target Pack =>
        targetDefinition => targetDefinition.DependsOn(Test)
                                            .Executes(
                                                 () =>
                                                 {
                                                     DotNetPack(packSettings => packSettings.SetProject(Solution).SetOutputDirectory(PackagesDirectory).SetIncludeSymbols(true).SetConfiguration(Configuration).EnableNoRestore().EnableNoBuild());
                                                 });

    AbsolutePath PackagesDirectory => RootDirectory / "packages";

    Target Restore =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                DotNetRestore(restoreSettings => restoreSettings.SetProjectFile(Solution));
            });

    Target RestoreWithDependencies =>
        targetDefinition => targetDefinition.DependsOn(CleanAll)
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
                                                     DotNetTest(testSettings => testSettings.SetProjectFile(Solution).SetConfiguration(Configuration).EnableNoRestore().EnableNoBuild());
                                                 });

    AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    // Support plugins are available for:
    // - JetBrains ReSharper        https://nuke.build/resharper
    // - JetBrains Rider            https://nuke.build/rider
    // - Microsoft VisualStudio     https://nuke.build/visualstudio
    // - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(build => build.Pack);

    /// <summary>Creates an action that performs the project's standard DotNet Clean task.</summary>
    /// <param name="configuration">The build configuration to use.</param>
    /// <returns>An action that performs the project's standard DotNet Clean task.</returns>
    Action DoStandardClean(Configuration configuration) =>
        () =>
        {
            DotNetClean(settings => settings.SetConfiguration(configuration).SetProject(Solution));
        };
}
