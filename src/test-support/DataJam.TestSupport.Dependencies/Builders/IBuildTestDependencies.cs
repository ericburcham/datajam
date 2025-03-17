namespace DataJam.TestSupport.Dependencies;

using JetBrains.Annotations;

[PublicAPI]
public interface IBuildTestDependencies<out T>
    where T : ITestDependency
{
    T Build();
}
