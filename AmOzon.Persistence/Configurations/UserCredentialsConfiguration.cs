using AmOzon.Persistence.Entities;
using AmOzon.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class UserCredentialsConfiguration : IEntityTypeConfiguration<UserCredentialsEntity>
{
    public void Configure(EntityTypeBuilder<UserCredentialsEntity> builder)
    {
        builder.ToTable("UserCredentials");
        
        builder
            .HasOne(uc => uc.User)
            .WithOne(u => u.UserCredentialsEntity)
            .HasForeignKey<UserCredentialsEntity>(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(uc => uc.UserName)
            .HasMaxLength(ValidationConstants.MaxUserNameLength)
            .IsRequired();
        builder.HasIndex(uc => uc.UserName)
            .IsUnique();
        
        builder.Property(uc => uc.Email)
            .HasMaxLength(ValidationConstants.MaxEmailLength)
            .IsRequired();
        builder.HasIndex(uc => uc.Email)
            .IsUnique();
        
        builder.Property(uc => uc.PasswordHash)
            .IsRequired();
    }
}
    
