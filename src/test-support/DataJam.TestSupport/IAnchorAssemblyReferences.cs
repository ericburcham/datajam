namespace DataJam.TestSupport;

using System.Reflection;

using JetBrains.Annotations;

[PublicAPI]
public interface IAnchorAssemblyReferences
{
    public Assembly AnchoredAssembly { get; }
}
