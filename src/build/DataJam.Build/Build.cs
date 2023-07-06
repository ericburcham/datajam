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
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)", Name = "Configuration")]
    readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Verbosity of the build - Default is 'Quiet'", Name = "DotNetVerbosity")]
    readonly DotNetVerbosity _verbosity = DotNetVerbosity.Quiet;

    [Solution]
    readonly Solution Solution = null!;

    /// <summary>Gets a target that runs DotNet Build for all build configurations.</summary>
    Target BuildAll => targetDefinition => targetDefinition.DependsOn(BuildDebug, BuildRelease);

    /// <summary>Gets a target that runs DotNet Build for all build configurations after running all dependencies.</summary>
    Target BuildAllWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, Restore, BuildDebug, BuildRelease);

    /// <summary>Gets a target that runs DotNet Build for the debug configuration.</summary>
    Target BuildDebug => targetDefinition => targetDefinition.After(CleanDebug, Restore).Executes(StandardBuild(Configuration.Debug));

    /// <summary>Gets a target that runs DotNet Build for the debug configuration after running all dependencies.</summary>
    Target BuildDebugWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanDebug, Restore, BuildDebug);

    /// <summary>Gets a target that runs DotNet Build for the Release configuration.</summary>
    Target BuildRelease => targetDefinition => targetDefinition.After(CleanRelease, Restore).Executes(StandardBuild(Configuration.Release));

    /// <summary>Gets a target that runs DotNet Build for the Release configuration after running all dependencies.</summary>
    Target BuildReleaseWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanRelease, Restore, BuildRelease);

    /// <summary>Gets a target that runs dotnet clean for all configurations, removes all bin and object folders, and ensures the package output folder is clean.</summary>
    Target CleanAll => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, CleanOutput, CleanPackages);

    /// <summary>Gets a target that runs dotnet clean for the debug configuration.</summary>
    Target CleanDebug => targetDefinition => targetDefinition.Executes(StandardClean(Configuration.Debug));

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
    Target CleanRelease => targetDefinition => targetDefinition.Executes(StandardClean(Configuration.Release));

    Target Default => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, Restore, BuildDebug, BuildRelease, TestDebug, TestRelease, Pack);

    Target Pack =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                DotNetPack(packSettings => packSettings.SetProject(Solution).SetOutputDirectory(PackagesDirectory).SetIncludeSymbols(true).SetConfiguration(Configuration.Release).EnableNoRestore().EnableNoBuild());
            });

    AbsolutePath PackagesDirectory => RootDirectory / "packages";

    /// <summary>Gets a target that restores package dependencies.</summary>
    Target Restore => targetDefinition => targetDefinition.After(CleanAll, CleanDebug, CleanOutput, CleanPackages, CleanRelease).Executes(StandardRestore());

    AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    Target TestAll => targetDefinition => targetDefinition.DependsOn(TestDebug, TestRelease);

    Target TestAllWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, Restore, BuildDebug, BuildRelease, TestDebug, TestRelease);

    Target TestDebug => targetDefinition => targetDefinition.After(CleanDebug, Restore, BuildDebug).Executes(StandardTest(Configuration.Debug));

    Target TestDebugWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanDebug, Restore, BuildDebug, TestDebug);

    Target TestRelease => targetDefinition => targetDefinition.After(CleanRelease, Restore, BuildRelease).Executes(StandardTest(Configuration.Release));

    Target TestReleaseWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanRelease, Restore, BuildRelease, TestRelease);

    AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    // Support plugins are available for:
    // - JetBrains ReSharper        https://nuke.build/resharper
    // - JetBrains Rider            https://nuke.build/rider
    // - Microsoft VisualStudio     https://nuke.build/visualstudio
    // - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(build => build.Default);

    /// <summary>Creates an action that performs the project's standard DotNet Build task.</summary>
    /// <param name="configuration">The build configuration to use.</param>
    /// <returns>An action that performs the project's standard DotNet Build task.</returns>
    Action StandardBuild(Configuration configuration) =>
        () =>
        {
            DotNetBuild(buildSettings => buildSettings.SetProjectFile(Solution).SetConfiguration(configuration).EnableNoRestore().SetVerbosity(_verbosity));
        };

    /// <summary>Creates an action that performs the project's standard DotNet Clean task.</summary>
    /// <param name="configuration">The build configuration to use.</param>
    /// <returns>An action that performs the project's standard DotNet Clean task.</returns>
    Action StandardClean(Configuration configuration) =>
        () =>
        {
            DotNetClean(settings => settings.SetConfiguration(configuration).SetProject(Solution).SetVerbosity(_verbosity));
        };

    /// <summary>Creates an action that performs the project's standard DotNet Restore task.</summary>
    /// <returns>An action that performs the project's standard DotNet Restore task.</returns>
    Action StandardRestore() =>
        () =>
        {
            DotNetRestore(restoreSettings => restoreSettings.SetProjectFile(Solution).SetVerbosity(_verbosity));
        };

    Action StandardTest(Configuration configuration) =>
        () =>
        {
            DotNetTest(testSettings => testSettings.SetProjectFile(Solution).SetConfiguration(configuration).EnableNoRestore().EnableNoBuild().EnableNoRestore().SetVerbosity(_verbosity));
        };
}
