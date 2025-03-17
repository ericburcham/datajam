namespace DataJam.TestSupport.Dependencies;

using System;
using System.Collections.Concurrent;

internal static class TestDependencyRegistry
{
    private static readonly ConcurrentDictionary<string, object> _testDependencies = new();

    public static void Add<T>(string name, T dependency)
        where T : class
    {
        if (!_testDependencies.TryAdd(name, dependency))
        {
            throw new ArgumentException($"A test dependency with the name \"{name}\" already exists in the {nameof(TestDependencyRegistry)}.");
        }
    }

    public static T Get<T>(string name)
    {
        return (T)_testDependencies[name];
    }
}
