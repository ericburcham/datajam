namespace DataJam.TestSupport.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public interface IProvideDbContextOptions
{
    DbContextOptions Options { get; }
}
