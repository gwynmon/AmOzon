using AmOzon.Persistence.Configurations;
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
    public DbSet<SellerEntity> Sellers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new SellerConfiguration());
    }
}