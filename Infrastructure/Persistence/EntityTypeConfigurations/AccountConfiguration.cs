using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityTypeConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("account_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(a => a.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("nvarchar(100)");

            builder.Property(a => a.ICNumber)
                .IsRequired()
                .HasColumnName("ic_number")
                .HasColumnType("nvarchar(20)");

            builder.Property(a => a.MobileNumber)
                .IsRequired()
                .HasColumnName("mobile_number")
                .HasColumnType("nvarchar(15)");

            builder.Property(a => a.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasColumnType("nvarchar(150)");

            builder.Property(a => a.PinHash)
                .HasColumnName("pin_hash")
                .HasColumnType("nvarchar(255)");

            builder.Property(a => a.MobileNumberConfirmed)
                .HasColumnName("mobile_number_confirmed")
                .HasColumnType("bit");

            builder.Property(a => a.EmailConfirmed)
                .HasColumnName("email_confirmed")
                .HasColumnType("bit");

            builder.Property(a => a.BiometricEnabled)
                .HasColumnName("biometric_enabled")
                .HasColumnType("bit");

            builder.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasIndex(a => a.ICNumber)
                .IsUnique();
        }
    }
}
