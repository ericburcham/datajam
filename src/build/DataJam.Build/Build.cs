// <copyright file="Build.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// � Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

/// <summary>
/// Exposes and coordinates <see cref="Target"/>s for use by Nuke Build.
/// </summary>
[UnsetVisualStudioEnvironmentVariables]
public class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration _configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    [Solution]
    readonly Solution Solution;

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean =>
        targetDefinition => targetDefinition
             .Executes(() =>
             {
                 SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                 TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                 EnsureCleanDirectory(ArtifactsDirectory);
             });

    Target Compile =>
        targetDefinition => targetDefinition
             .DependsOn(Restore)
             .Executes(() =>
             {
                 DotNetBuild(buildSettings => buildSettings
                                  .SetProjectFile(Solution)
                                  .SetConfiguration(_configuration)
                                  .EnableNoRestore());
             });

    Target Pack =>
        targetDefinition => targetDefinition
             .DependsOn(Test)
             .Executes(() =>
             {
                 DotNetPack(packSettings => packSettings
                                 .SetProject(Solution)
                                 .SetOutputDirectory(ArtifactsDirectory)
                                 .SetIncludeSymbols(true)
                                 .SetConfiguration(_configuration)
                                 .EnableNoRestore()
                                 .EnableNoBuild());
             });

    Target Restore =>
        targetDefinition => targetDefinition
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetRestore(restoreSettings => restoreSettings
                    .SetProjectFile(Solution));
            });

    AbsolutePath SourceDirectory => RootDirectory / "src" / "code";

    Target Test =>
        targetDefinition => targetDefinition
             .DependsOn(Compile)
             .Executes(() =>
             {
                 DotNetTest(testSettings => testSettings
                                 .SetProjectFile(Solution)
                                 .SetConfiguration(_configuration)
                                 .EnableNoRestore()
                                 .EnableNoBuild());
             });

    AbsolutePath TestsDirectory => RootDirectory / "src" / "tests";

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(build => build.Pack);
}
