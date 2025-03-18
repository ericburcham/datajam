using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    Target Clean =>
        _ => _
            .Before(Restore)
            .Executes(() =>
             {
                 DotNetClean(_ => _.SetProject(Solution.Path));
             });

    Target Compile =>
        _ => _
            .DependsOn(Restore)
            .Executes(() =>
             {
                 DotNetBuild(x => x.SetProjectFile(Solution));
             });

    Target Restore =>
        _ => _
            .DependsOn(Clean)
            .Executes(() =>
             {
                 DotNetRestore(x => x.SetProjectFile(Solution));
             });

    Target Test =>
        _ => _
            .DependsOn(Compile)
            .Executes(() =>
             {
                 DotNetTest(x => x.SetProjectFile(Solution));
             });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Test);
}
