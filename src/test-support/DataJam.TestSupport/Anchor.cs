namespace DataJam.TestSupport;

using System.Reflection;

using JetBrains.Annotations;

[PublicAPI]
public abstract class Anchor(Assembly anchoredAssembly) : IAnchorAssemblyReferences
{
    Assembly IAnchorAssemblyReferences.AnchoredAssembly => anchoredAssembly;
}

[PublicAPI]
public abstract class Anchor<TAnchor>() : Anchor(AnchoredAssembly)
{
    static Anchor()
    {
        AnchoredAssembly = typeof(TAnchor).Assembly;
    }

    // The below inspection is disable because one instance per derived Type is exactly what we want, here.
    // ReSharper disable once StaticMemberInGenericType
    public static Assembly AnchoredAssembly { get; }
}
