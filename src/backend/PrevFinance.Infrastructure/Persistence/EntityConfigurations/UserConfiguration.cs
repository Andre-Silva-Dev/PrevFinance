using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrevFinance.Domain.Users;

namespace PrevFinance.Infrastructure.Persistence.EntityConfigurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(512);

        builder.Property(x => x.ExternalProvider)
            .HasMaxLength(64);

        builder.Property(x => x.ExternalSubject)
            .HasMaxLength(200);

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasIndex(x => new { x.ExternalProvider, x.ExternalSubject })
            .IsUnique()
            .HasFilter("\"ExternalProvider\" IS NOT NULL AND \"ExternalSubject\" IS NOT NULL");
    }
}
