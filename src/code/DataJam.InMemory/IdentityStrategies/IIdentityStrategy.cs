namespace DataJam.InMemory.IdentityStrategies;

public interface IIdentityStrategy<in T>
    where T : class
{
    void Assign(T entity);
}