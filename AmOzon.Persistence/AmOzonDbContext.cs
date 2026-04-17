using AmOzon.Persistence.Configurations;
using AmOzon.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence;

public class AmOzonDbContext : IdentityDbContext<UserCredentialsEntity, IdentityRole<Guid>, Guid>
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}