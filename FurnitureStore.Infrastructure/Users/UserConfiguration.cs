using FurnitureStore.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureStore.Infrastructure.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user", "public");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("id")
            .HasColumnType("guid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(user => user.Username)
            .HasColumnName("username")
            .HasColumnType("character varying")
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasColumnType("character varying")
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasColumnType("binary(32)")
            .IsRequired();
        
        builder.Property(user => user.PasswordSalt)
            .HasColumnName("password_salt")
            .HasColumnType("binary(32)")
            .IsRequired();

        builder.Property(user => user.AccessToken)
            .HasColumnName("access_token")
            .HasColumnType("character_varying")
            .IsRequired(false);

        builder.Property(user => user.VerifiedAt)
            .HasColumnName("verified_at")
            .HasColumnType("datetime")
            .IsRequired(false);

        builder.Property(user => user.PasswordResetToken)
           .HasColumnName("password_reset_token")
           .HasColumnType("character_varying")
           .IsRequired(false);

        builder.Property(user => user.ResetTokenExpires)
            .HasColumnName("reset_token_expires")
            .HasColumnType("datetime")
            .IsRequired(false);
    }
}
