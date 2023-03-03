namespace DataJam.Testing.UnitTests.QuickAndDirty.Domain
{
    public class IdentifiablePerson<T> : IIdentifiable<T>
        where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}
