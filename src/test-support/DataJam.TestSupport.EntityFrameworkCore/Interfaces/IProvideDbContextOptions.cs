namespace DataJam.TestSupport.EntityFrameworkCore;

public interface IProvideDbContextOptions
{
    TransactionalDbContextOptions Options { get; }
}
