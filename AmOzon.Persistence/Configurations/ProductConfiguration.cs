using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(x => x.ProductId);
        
        builder.Property(x => x.Name).IsRequired()
            .HasMaxLength(Product.MAX_NAME_LENGTH)
            .IsRequired();
        
        // More checks!
    }
}