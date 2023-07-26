namespace DataJam.Build;

using System;
using System.IO;
using System.Reflection;

using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ValueInjection;

public class VersionInfoAttribute : ValueInjectionAttributeBase
{
    private const string VERSION_INFO_FILENAME = "version-info.txt";

    private readonly GitRepository _repository;

    private readonly AbsolutePath _rootDirectory;

    private readonly string? _spaceExecutionNumber;

    public VersionInfoAttribute()
    {
        _rootDirectory = NukeBuild.RootDirectory;
        _repository = GitRepository.FromLocalDirectory(_rootDirectory);
        _spaceExecutionNumber = Environment.GetEnvironmentVariable("JB_SPACE_EXECUTION_NUMBER");
    }

    private string VersionPrefix
    {
        get
        {
            var rootDirectory = _rootDirectory.ToString();
            var versionInfoFilePath = Path.Combine(rootDirectory, VERSION_INFO_FILENAME);

            return File.Exists(versionInfoFilePath) ? File.ReadAllText(versionInfoFilePath) : "0.0.1";
        }
    }

    private string VersionSuffix
    {
        get
        {
            var buildNumber = GetBuildNumber();

            if (_repository.IsOnMainBranch())
            {
                return string.Empty;
            }

            return _repository.IsOnReleaseBranch() ? $"beta{buildNumber}" : $"alpha{buildNumber}";
        }
    }

    public override object GetValue(MemberInfo member, object instance) => new VersionInfo(VersionPrefix, VersionSuffix);

    private string GetBuildNumber() => GetBuildNumberFromSpace() ?? GetBuildNumberFromDateAndTime();

    private string GetBuildNumberFromDateAndTime()
    {
        var now = DateTime.UtcNow;
        var currentYear = now.ToString("yy");
        var dayOfYear = now.DayOfYear;

        // [2-digit year][day of year]
        var dateStamp = $"{currentYear}{dayOfYear}";

        // [2-digit hour][2-digit minute][2-digit second]
        var timeStamp = now.ToString("HHmmss");

        return $"{dateStamp}{timeStamp}";
    }

    private string? GetBuildNumberFromSpace() => _spaceExecutionNumber ?? null;
}
