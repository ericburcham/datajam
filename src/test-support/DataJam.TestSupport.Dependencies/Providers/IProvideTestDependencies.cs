namespace DataJam.TestSupport.Dependencies;

using System.Collections.Generic;

using JetBrains.Annotations;

[PublicAPI]
public interface IProvideTestDependencies
{
    IEnumerable<ITestDependency> TestDependencies { get; }
}
