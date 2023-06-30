namespace DataJam.TestSupport;

using DotNet.Testcontainers.Containers;

public interface IProvideContainers
{
    IEnumerable<IContainer> Containers { get; }
}
