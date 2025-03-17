namespace DataJam.TestSupport.Dependencies;

using JetBrains.Annotations;

[PublicAPI]
public abstract class TestDependencyBuilder<T> : IBuildTestDependencies<T>
    where T : ITestDependency
{
    public abstract T Build();
}
