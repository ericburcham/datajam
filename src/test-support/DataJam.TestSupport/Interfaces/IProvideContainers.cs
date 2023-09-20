namespace DataJam.TestSupport;

using System.Collections.Generic;

using DotNet.Testcontainers.Containers;

public interface IProvideContainers
{
    IEnumerable<IContainer> Containers { get; }
}
