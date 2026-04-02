using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class SellerConfiguration : IEntityTypeConfiguration<SellerEntity>
{
    public void Configure(EntityTypeBuilder<SellerEntity> builder)
    {
        builder
            .HasKey(s => s.Id);
        
        builder
            .HasOne(s => s.User)
            .WithOne(u => u.Seller)
            .HasForeignKey<SellerEntity>(s => s.UserId);
    }
}