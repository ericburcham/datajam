using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities;

using Serilog;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

[DotNetVerbosityMapping]
[GitHubActions("nuke-build",
               GitHubActionsImage.UbuntuLatest,

               // FetchDepth is important for GitVersion
               FetchDepth = 0,
               On = [GitHubActionsTrigger.Push],
               ImportSecrets = [nameof(NuGetApiKey)],
               InvokedTargets = [nameof(Default)])]
[HandleSingleFileExecution]
[ShutdownDotNetAfterServerBuild]
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
            .Executes(() => GitVersion.PrintGitVersionInfo());

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

    Target Default => x => x.DependsOn(InstallAdditionalSdks, Pack, Publish);

    Target InstallAdditionalSdks =>
        x => x
            .Before(Clean)
            .Executes(() =>
             {
                 var additionalSdkVersions = GetAdditionalSdkVersions();

                 if (additionalSdkVersions == null)
                 {
                     Log.Information("No additional SDKs found.");

                     return;
                 }

                 var versions = additionalSdkVersions.ToList();

                 if (versions.Count == 0)
                 {
                     Log.Information("No additional SDKs found.");

                     return;
                 }

                 Log.Information("Additional SDKs found:");

                 // Build the path to the dotnet install script
                 var environmentIsWindows = EnvironmentInfo.IsWin;
                 var scriptFileName = environmentIsWindows ? "dotnet-install.ps1" : "dotnet-install.sh";
                 var globalNukeDirectory = EnvironmentInfo.SpecialFolder(SpecialFolders.UserProfile) / ".nuke";
                 var scriptFile = globalNukeDirectory / scriptFileName;

                 // Download the script file
                 var scriptUrl = $"https://dot.net/v1/{scriptFileName}";
                 Log.Information($"Downloading .NET SDK install script: {scriptUrl}");
                 HttpTasks.HttpDownloadFile(scriptUrl, scriptFile);

                 // Install SDKs
                 foreach (var version in versions)
                 {
                     Log.Information($"Installing .NET SDK version {version}");

                     ProcessTasks.StartShell(environmentIsWindows
                                                 ? $"powershell {scriptFile.ToString().DoubleQuoteIfNeeded()} dotnet -Version {version}"
                                                 : $"{scriptFile.ToString().DoubleQuoteIfNeeded()} dotnet --version {version}",
                                             logOutput: false)
                                 .AssertZeroExitCode();
                 }
             })
            .ProceedAfterFailure();

    Target Pack =>
        x => x
            .DependsOn(Test)
            .Executes(() =>
             {
                 var version = GitVersion.GetPackageVersion(ShouldPublish);

                 DotNetPack(o => o
                                .SetConfiguration(Configuration)
                                .SetNoBuild(true)
                                .SetOutputDirectory(ArtifactsDirectory)
                                .SetProject(Solution)
                                .SetVersion(version));

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

    private static IEnumerable<string> GetAdditionalSdkVersions()
    {
        var additionalSdksFile = RootDirectory / "additional-sdks.json";
        var jObject = additionalSdksFile.Existing()?.ReadJson() ?? new JObject();

        return jObject["sdks"]?["versions"]?.Values<string>() ?? [];
    }

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
}
