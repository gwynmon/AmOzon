using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence;

public class AmOzonDbContext : DbContext
{
    public AmOzonDbContext(DbContextOptions<AmOzonDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<ProductEntity> Products { get; set; }
}