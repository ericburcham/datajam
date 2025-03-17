namespace DataJam.TestSupport.Dependencies;

public abstract class TestDependency<T>(T dependency) : ITestDependency<T>
    where T : class
{
    public T Dependency { get; } = dependency;

    object ITestDependency.Dependency => Dependency;
}
