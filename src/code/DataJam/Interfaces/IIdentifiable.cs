namespace DataJam;

public interface IIdentifiable<T>
    where T : IEquatable<T>
{
    public T Id { get; set; }
}