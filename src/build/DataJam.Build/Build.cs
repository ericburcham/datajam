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

    /// <summary>Gets the absolute path for nuget package output.</summary>
    static AbsolutePath PackagesDirectory => RootDirectory / "packages";

    /// <summary>Gets the absolute path for the project's source code.</summary>
    static AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    /// <summary>Gets the absolute path for the project's test folder.</summary>
    static AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

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

    /// <summary>Gets a target that runs DotNet Clean for all configurations, removes all bin and object folders, and ensures the package output folder is clean.</summary>
    Target CleanAll => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, CleanOutput, CleanPackages);

    /// <summary>Gets a target that runs DotNet Clean for the debug configuration.</summary>
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

    /// <summary>Gets a target that runs DotNet Clean for the release configuration.</summary>
    Target CleanRelease => targetDefinition => targetDefinition.Executes(StandardClean(Configuration.Release));

    /// <summary>Gets a target that runs the default build.</summary>
    Target Default => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, Restore, BuildDebug, BuildRelease, TestDebug, TestRelease, Pack);

    /// <summary>Gets a target that runs DotNet Pack for the release configuration.</summary>
    Target Pack =>
        targetDefinition => targetDefinition.Executes(
            () =>
            {
                DotNetPack(packSettings => packSettings.SetProject(Solution).SetOutputDirectory(PackagesDirectory).EnableIncludeSymbols().SetConfiguration(Configuration.Release).EnableNoRestore().EnableNoBuild());
            });

    /// <summary>Gets a target that restores package dependencies.</summary>
    Target Restore => targetDefinition => targetDefinition.After(CleanAll, CleanDebug, CleanOutput, CleanPackages, CleanRelease).Executes(StandardRestore());

    /// <summary>Gets a target that runs DotNet Test for all build configurations.</summary>
    Target TestAll => targetDefinition => targetDefinition.DependsOn(TestDebug, TestRelease);

    /// <summary>Gets a target that runs DotNet Test for all build configurations after running all dependencies.</summary>
    Target TestAllWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanDebug, CleanRelease, Restore, BuildDebug, BuildRelease, TestDebug, TestRelease);

    /// <summary>Gets a target that runs DotNet Test for the debug configuration.</summary>
    Target TestDebug => targetDefinition => targetDefinition.After(CleanDebug, Restore, BuildDebug).Executes(StandardTest(Configuration.Debug));

    /// <summary>Gets a target that runs DotNet Test for the debug configuration after running all dependencies.</summary>
    Target TestDebugWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanDebug, Restore, BuildDebug, TestDebug);

    /// <summary>Gets a target that runs DotNet Test for the release configuration.</summary>
    Target TestRelease => targetDefinition => targetDefinition.After(CleanRelease, Restore, BuildRelease).Executes(StandardTest(Configuration.Release));

    /// <summary>Gets a target that runs DotNet test for the release configuration after running all dependencies.</summary>
    Target TestReleaseWithDependencies => targetDefinition => targetDefinition.DependsOn(CleanRelease, Restore, BuildRelease, TestRelease);

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
