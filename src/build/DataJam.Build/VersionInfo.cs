namespace DataJam.Build;

public class VersionInfo(string versionPrefix, string versionSuffix)
{
    public string AssemblyVersion => VersionPrefix;

    public string FileVersion => AssemblyVersion;

    public string InformationalVersion => Version;

    public string PackageVersion => Version;

    public string Version
    {
        get
        {
            if (string.IsNullOrEmpty(VersionSuffix))
            {
                return VersionPrefix;
            }

            return $"{VersionPrefix}-{VersionSuffix}";
        }
    }

    public string VersionPrefix { get; } = versionPrefix;

    public string VersionSuffix { get; } = versionSuffix;
}
