using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;

using Serilog;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions("continuous",
               GitHubActionsImage.UbuntuLatest,
               On = [GitHubActionsTrigger.Push],
               InvokedTargets = [nameof(Default)])]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitRepository] readonly GitRepository GitRepository;

    [Solution] readonly Solution Solution;

    [GitVersion] GitVersion GitVersion;

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean =>
        x => x
            .Before(Restore)
            .Executes(() =>
             {
                 ArtifactsDirectory.CreateOrCleanDirectory();

                 DotNetClean(o => o
                                 .SetConfiguration(Configuration)
                                 .SetProject(Solution.Path));
             });

    Target Compile =>
        x => x
            .DependsOn(Restore)
            .Executes(() =>
             {
                 DotNetBuild(o => o
                                 .SetConfiguration(Configuration)
                                 .SetNoRestore(true)
                                 .SetProjectFile(Solution));
             });

    Target Default => x => x.DependsOn(Pack);

    Target Pack =>
        x => x
            .DependsOn(Test)
            .Executes(() =>
             {
                 DotNetPack(o => o
                                .SetConfiguration(Configuration)
                                .SetNoBuild(true)
                                .SetOutputDirectory(ArtifactsDirectory)
                                .SetProject(Solution));
             })
            .Produces(ArtifactsDirectory / "*.nupkg");

    Target PrintVersion =>
        x => x
           .Executes(() =>
            {
                Log.Information($"GitVersion = {GitVersion.MajorMinorPatch}");
            });

    Target Publish =>
        x => x
            .DependsOn(Pack)
            .TriggeredBy(Pack)
            .Executes(() =>
             {
                 DotNetPublish(o => o
                                   .SetConfiguration(Configuration)
                                   .SetNoBuild(true)
                                   .SetProject(Solution));
             })
            .OnlyWhenStatic(() => ShouldPublish);

    Target Restore =>
        x => x
            .DependsOn(Clean)
            .Executes(() =>
             {
                 DotNetToolRestore();

                 DotNetRestore(o => o
                                  .SetProjectFile(Solution));
             });

    bool ShouldPublish => GitRepository.IsOnMainOrMasterBranch();

    Target Test =>
        x => x
            .DependsOn(Compile)
            .Executes(() =>
             {
                 DotNetTest(o => o
                                .SetConfiguration(Configuration)
                                .SetNoBuild(true)
                                .SetProjectFile(Solution));
             });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Default);
}
