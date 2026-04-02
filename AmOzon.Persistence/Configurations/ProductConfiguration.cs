using AmOzon.Domain.Constants;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder
            .HasKey(p => p.Id);
        

        builder.HasOne(p => p.Seller)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SellerId)
            .IsRequired();
        
        builder.Property(p => p.Name)
            .HasMaxLength(ValidationConstants.MaxProductNameLength)
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(ValidationConstants.MaxDescriptionLength)
            .IsRequired();
        
        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasIndex(p => p.SellerId);
        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => p.Price);
        
        // More checks!
    }
}