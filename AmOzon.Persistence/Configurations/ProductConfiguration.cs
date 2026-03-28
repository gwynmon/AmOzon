using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Seller)
            .WithMany(s => s.Products);
        
        builder.Property(p => p.Name)
            .HasMaxLength(Product.MAX_NAME_LENGTH)
            .IsRequired();
        
        // More checks!
    }
}