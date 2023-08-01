namespace DataJam.TestSupport;

using DotNet.Testcontainers.Containers;

public interface IRegisterContainers
{
    void Register(IContainer container);
}
