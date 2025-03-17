namespace DataJam.TestSupport.Dependencies;

using JetBrains.Annotations;

[PublicAPI]
public static class RegisteredTestDependencies
{
    public static T Get<T>(string name)
        where T : class
    {
        return TestDependencyRegistry.Get<T>(name);
    }
}
