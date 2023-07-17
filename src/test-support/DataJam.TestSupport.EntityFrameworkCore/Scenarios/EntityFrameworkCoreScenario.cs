namespace DataJam.TestSupport.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public abstract class EntityFrameworkCoreScenario : TransactionalScenario
{
    protected abstract DbContextOptions DbContextOptions { get; }
}
