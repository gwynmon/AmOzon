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
            .HasMany(s => s.ProductsInCart)
            .WithOne(u => u.User)
            .HasForeignKey(ci => ci.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(u => u.UserCredentialsEntity)
            .WithOne(uc => uc.User)
            .HasForeignKey<UserCredentialsEntity>(uc => uc.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Property(u => u.Name)
            .HasMaxLength(ValidationConstants.MaxUserNameLength)
            .IsRequired();
        
        builder
            .Property(u => u.Age)
            .IsRequired();
    }
}