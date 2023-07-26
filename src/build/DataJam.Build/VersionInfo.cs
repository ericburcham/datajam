namespace DataJam.Build;

public class VersionInfo
{
    public VersionInfo(string versionPrefix, string versionSuffix)
    {
        VersionPrefix = versionPrefix;
        VersionSuffix = versionSuffix;
    }

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

    public string VersionPrefix { get; }

    public string VersionSuffix { get; }
}
