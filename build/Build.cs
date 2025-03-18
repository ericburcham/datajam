using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions("continuous",
               GitHubActionsImage.UbuntuLatest,
               On = [GitHubActionsTrigger.Push],
               InvokedTargets = [nameof(Default)])]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    Target Clean =>
        x => x
            .Before(Restore)
            .Executes(() =>
             {
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
                                 .SetProjectFile(Solution));
             });

    Target Default => x => x.DependsOn(Test);

    Target Restore =>
        x => x
            .DependsOn(Clean)
            .Executes(() =>
             {
                 DotNetRestore(o => o
                                  .SetProjectFile(Solution));
             });

    Target Test =>
        x => x
            .DependsOn(Compile)
            .Executes(() =>
             {
                 DotNetTest(o => o
                                .SetConfiguration(Configuration)
                                .SetProjectFile(Solution));
             });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Test);
}
