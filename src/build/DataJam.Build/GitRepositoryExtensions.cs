namespace DataJam.Build;

using Nuke.Common.Git;
using Nuke.Common.Utilities;

internal static class GitRepositoryExtensions
{
    private const string RELEASE_BRANCH_PREFIX = "release/";

    public static bool IsOnReleaseBranch(this GitRepository repository) => repository.Branch?.StartsWithOrdinalIgnoreCase(RELEASE_BRANCH_PREFIX) ?? false;
}
