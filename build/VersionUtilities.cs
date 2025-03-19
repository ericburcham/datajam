using Nuke.Common.Tools.GitVersion;

using Serilog;

internal static class VersionUtilities
{
    public static string GetPackageVersion(this GitVersion gitVersion, bool shouldPublish) => shouldPublish ? gitVersion.MajorMinorPatch : gitVersion.FullSemVer;

    public static void PrintGitVersionInfo(this GitVersion gitVersion)
    {
        Log.Information($"AssemblySemFileVer: {gitVersion.AssemblySemFileVer}");
        Log.Information($"AssemblySemVer: {gitVersion.AssemblySemVer}");
        Log.Information($"BranchName: {gitVersion.BranchName}");
        Log.Information($"BuildMetaData: {gitVersion.BuildMetaData}");
        Log.Information($"BuildMetaDataPadded: {gitVersion.BuildMetaDataPadded}");
        Log.Information($"CommitDate: {gitVersion.CommitDate}");
        Log.Information($"CommitsSinceVersionSource: {gitVersion.CommitsSinceVersionSource}");
        Log.Information($"CommitsSinceVersionSourcePadded: {gitVersion.CommitsSinceVersionSourcePadded}");
        Log.Information($"EscapedBranchName: {gitVersion.EscapedBranchName}");
        Log.Information($"FullBuildMetaData: {gitVersion.FullBuildMetaData}");
        Log.Information($"FullSemVer: {gitVersion.FullSemVer}");
        Log.Information($"InformationalVersion: {gitVersion.InformationalVersion}");
        Log.Information($"InformationalVersion: {gitVersion.InformationalVersion}");
        Log.Information($"LegacySemVer: {gitVersion.LegacySemVer}");
        Log.Information($"LegacySemVerPadded: {gitVersion.LegacySemVerPadded}");
        Log.Information($"Major: {gitVersion.Major}");
        Log.Information($"MajorMinorPatch: {gitVersion.MajorMinorPatch}");
        Log.Information($"Minor: {gitVersion.Minor}");
        Log.Information($"NuGetPreReleaseTag: {gitVersion.NuGetPreReleaseTag}");
        Log.Information($"NuGetPreReleaseTagV2: {gitVersion.NuGetPreReleaseTagV2}");
        Log.Information($"NuGetVersion: {gitVersion.NuGetVersion}");
        Log.Information($"NuGetVersionV2: {gitVersion.NuGetVersionV2}");
        Log.Information($"Patch: {gitVersion.Patch}");
        Log.Information($"PreReleaseLabel: {gitVersion.PreReleaseLabel}");
        Log.Information($"PreReleaseLabelWithDash: {gitVersion.PreReleaseLabelWithDash}");
        Log.Information($"PreReleaseNumber: {gitVersion.PreReleaseNumber}");
        Log.Information($"PreReleaseTag: {gitVersion.PreReleaseTag}");
        Log.Information($"PreReleaseTagWithDash: {gitVersion.PreReleaseTagWithDash}");
        Log.Information($"SemVer: {gitVersion.SemVer}");
        Log.Information($"Sha: {gitVersion.Sha}");
        Log.Information($"ShortSha: {gitVersion.ShortSha}");
        Log.Information($"UncommittedChanges: {gitVersion.UncommittedChanges}");
        Log.Information($"VersionSourceSha: {gitVersion.VersionSourceSha}");
        Log.Information($"WeightedPreReleaseNumber: {gitVersion.WeightedPreReleaseNumber}");
    }
}
