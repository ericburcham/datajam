namespace DataJam.TestSupport.TestContainers;

using System.Collections.Generic;

using DotNet.Testcontainers.Containers;

public interface IProvideTestContainers
{
    IEnumerable<IContainer> TestContainers { get; }
}
