using AmOzon.Persistence.Entities;
using AmOzon.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmOzon.Persistence.Configurations;

public class UserCredentialsConfiguration : IEntityTypeConfiguration<UserCredentialsEntity>
{
    public void Configure(EntityTypeBuilder<UserCredentialsEntity> builder)
    {
        builder
            .HasKey(uc => uc.UserId);
        
        builder
            .HasOne(uc => uc.User)
            .WithOne(u => u.UserCredentialsEntity)
            .HasForeignKey<UserCredentialsEntity>(uc => uc.UserId);
        
        builder.Property(uc => uc.Email)
            .HasMaxLength(ValidationConstants.MaxEmailLength)
            .IsRequired();
        
        builder.Property(uc => uc.Password)
            .HasMaxLength(ValidationConstants.MaxPasswordLength)
            .IsRequired();
        
        builder.HasIndex(uc => uc.Email)
            .IsUnique();
    }
}
    
