using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;

using Serilog;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions("nuke-build",
               GitHubActionsImage.UbuntuLatest,

               // FetchDepth is important for GitVersion
               FetchDepth = 0,
               On = [GitHubActionsTrigger.Push],
               ImportSecrets = [nameof(NuGetApiKey)],
               InvokedTargets = [nameof(Default)])]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitRepository] readonly GitRepository GitRepository;

    [GitVersion] readonly GitVersion GitVersion;

    [Parameter("NuGet API Key for publishing packages")] [Secret] readonly string NuGetApiKey;

    [Solution] readonly Solution Solution;

    Target AnnounceGitVersion =>
        x => x
            .Before(Clean)
            .Executes(PrintVersion);

    Target AnnounceNuGetApiKey =>
        x => x
            .Before(AnnounceGitVersion)
            .Executes(() =>
             {
                 var valueName = nameof(NuGetApiKey);

                 if (string.IsNullOrEmpty(NuGetApiKey))
                 {
                     Log.Information($"{valueName} is not defined.");
                 }
                 else
                 {
                     var maskedValue = MaskString(NuGetApiKey, 4);
                     Log.Information($"{valueName} is defined: {maskedValue}.");
                 }
             });

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
            .DependsOn(AnnounceNuGetApiKey, AnnounceGitVersion, Restore)
            .Executes(() =>
             {
                 DotNetBuild(o => o
                                 .SetAssemblyVersion(GitVersion.AssemblySemVer)
                                 .SetConfiguration(Configuration)
                                 .SetFileVersion(GitVersion.AssemblySemFileVer)
                                 .SetInformationalVersion(GitVersion.InformationalVersion)
                                 .SetNoRestore(true)
                                 .SetProjectFile(Solution)
                                 .SetVersion(GitVersion.SemVer));
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
                                .SetProject(Solution)
                                .SetVersion(GitVersion.FullSemVer));

                 Log.Information($"Produced NuGet package version {GitVersion.NuGetVersion}");
             })
            .Produces(ArtifactsDirectory / "*.nupkg");

    Target Publish =>
        x => x
            .DependsOn(Pack)
            .Executes(() =>
             {
                 // If you want to publish to NuGet.org, include the NuGetApiKey
                 if (string.IsNullOrEmpty(NuGetApiKey))
                 {
                     return;
                 }

                 var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

                 DotNetNuGetPush(s => s
                                     .SetSource("https://api.nuget.org/v3/index.json")
                                     .SetApiKey(NuGetApiKey)
                                     .CombineWith(packages, (cs, v) => cs.SetTargetPath(v)));

                 Log.Information($"Published {packages.Count} packages to NuGet");
             })
            .OnlyWhenStatic(() => ShouldPublish)
            .TriggeredBy(Pack);

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

    private static string MaskString(string value, int clear)
    {
        if (clear >= value.Length)
        {
            return value;
        }

        var maskLength = value.Length - clear;
        var maskedPart = new string('*', maskLength);
        var clearPart = value.Substring(maskLength);

        return maskedPart + clearPart;
    }

    private void PrintVersion()
    {
        Log.Information($"AssemblySemFileVer: {GitVersion.AssemblySemFileVer}");
        Log.Information($"AssemblySemVer: {GitVersion.AssemblySemVer}");
        Log.Information($"BranchName: {GitVersion.BranchName}");
        Log.Information($"BuildMetaData: {GitVersion.BuildMetaData}");
        Log.Information($"BuildMetaDataPadded: {GitVersion.BuildMetaDataPadded}");
        Log.Information($"CommitDate: {GitVersion.CommitDate}");
        Log.Information($"CommitsSinceVersionSource: {GitVersion.CommitsSinceVersionSource}");
        Log.Information($"CommitsSinceVersionSourcePadded: {GitVersion.CommitsSinceVersionSourcePadded}");
        Log.Information($"EscapedBranchName: {GitVersion.EscapedBranchName}");
        Log.Information($"FullBuildMetaData: {GitVersion.FullBuildMetaData}");
        Log.Information($"FullSemVer: {GitVersion.FullSemVer}");
        Log.Information($"InformationalVersion: {GitVersion.InformationalVersion}");
        Log.Information($"InformationalVersion: {GitVersion.InformationalVersion}");
        Log.Information($"LegacySemVer: {GitVersion.LegacySemVer}");
        Log.Information($"LegacySemVerPadded: {GitVersion.LegacySemVerPadded}");
        Log.Information($"Major: {GitVersion.Major}");
        Log.Information($"MajorMinorPatch: {GitVersion.MajorMinorPatch}");
        Log.Information($"Minor: {GitVersion.Minor}");
        Log.Information($"NuGetPreReleaseTag: {GitVersion.NuGetPreReleaseTag}");
        Log.Information($"NuGetPreReleaseTagV2: {GitVersion.NuGetPreReleaseTagV2}");
        Log.Information($"NuGetVersion: {GitVersion.NuGetVersion}");
        Log.Information($"NuGetVersionV2: {GitVersion.NuGetVersionV2}");
        Log.Information($"Patch: {GitVersion.Patch}");
        Log.Information($"PreReleaseLabel: {GitVersion.PreReleaseLabel}");
        Log.Information($"PreReleaseLabelWithDash: {GitVersion.PreReleaseLabelWithDash}");
        Log.Information($"PreReleaseNumber: {GitVersion.PreReleaseNumber}");
        Log.Information($"PreReleaseTag: {GitVersion.PreReleaseTag}");
        Log.Information($"PreReleaseTagWithDash: {GitVersion.PreReleaseTagWithDash}");
        Log.Information($"SemVer: {GitVersion.SemVer}");
        Log.Information($"Sha: {GitVersion.Sha}");
        Log.Information($"ShortSha: {GitVersion.ShortSha}");
        Log.Information($"UncommittedChanges: {GitVersion.UncommittedChanges}");
        Log.Information($"VersionSourceSha: {GitVersion.VersionSourceSha}");
        Log.Information($"WeightedPreReleaseNumber: {GitVersion.WeightedPreReleaseNumber}");
    }
}
