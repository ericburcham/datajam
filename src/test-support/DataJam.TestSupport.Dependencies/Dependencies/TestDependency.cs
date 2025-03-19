namespace DataJam.TestSupport.Dependencies;

using JetBrains.Annotations;

[PublicAPI]
public abstract class TestDependency<T>(T dependency) : ITestDependency
    where T : class
{
    public T Dependency { get; } = dependency;

    object ITestDependency.Dependency => Dependency;
}
