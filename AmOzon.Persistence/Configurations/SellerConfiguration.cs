using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class SellerConfiguration : IEntityTypeConfiguration<SellerEntity>
{
    public void Configure(EntityTypeBuilder<SellerEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .HasMany(p => p.Products)
            .WithOne(p => p.Seller);
        
        // More checks!
    }
}