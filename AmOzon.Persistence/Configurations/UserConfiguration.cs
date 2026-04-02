using AmOzon.Domain.Constants;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasKey(u => u.Id);
        
        builder
            .HasMany(s => s.ProductsInCart);
        
        builder
            .Property(u => u.Name)
            .HasMaxLength(ValidationConstants.MaxUserNameLength)
            .IsRequired();
        
        builder
            .Property(u => u.Age)
            .IsRequired();
    }
}