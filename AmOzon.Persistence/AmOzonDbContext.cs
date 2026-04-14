using AmOzon.Domain.Models;
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
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<UserCredentialsEntity> UserCredentials { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}