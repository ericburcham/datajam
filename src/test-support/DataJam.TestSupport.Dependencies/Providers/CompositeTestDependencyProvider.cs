namespace DataJam.TestSupport.Dependencies;

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

[PublicAPI]
public abstract class CompositeTestDependencyProvider : IProvideTestDependencies
{
    private readonly Dictionary<string, ITestDependency> _testDependencies = new();

    public IEnumerable<ITestDependency> TestDependencies => _testDependencies.Values;

    protected void Register<T>(string name, IBuildTestDependencies<T> builder)
        where T : ITestDependency
    {
        var dependency = builder.Build();

        if (!_testDependencies.TryAdd(name, dependency))
        {
            throw new ArgumentException($"A test dependency with the name \'{name}\' already exists in the {GetType().Name}.");
        }

        TestDependencyRegistry.Add(name, dependency);
    }
}
