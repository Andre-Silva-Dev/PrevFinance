using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Users;

namespace PrevFinance.Infrastructure.Persistence.EntityConfigurations;

internal sealed class AccountBalanceAdjustmentConfiguration : IEntityTypeConfiguration<AccountBalanceAdjustment>
{
    public void Configure(EntityTypeBuilder<AccountBalanceAdjustment> builder)
    {
        builder.ToTable("account_balance_adjustments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PreviousBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.NewBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasMaxLength(240)
            .IsRequired();

        builder.Property(x => x.AdjustedAtUtc)
            .HasColumnName("adjusted_at_utc")
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.AccountId, x.AdjustedAtUtc });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
