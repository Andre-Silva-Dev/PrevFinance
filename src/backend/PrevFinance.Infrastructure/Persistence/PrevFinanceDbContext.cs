using Microsoft.EntityFrameworkCore;
using PrevFinance.Application.Abstractions;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Common;
using PrevFinance.Domain.Transactions;
using PrevFinance.Domain.Users;

namespace PrevFinance.Infrastructure.Persistence;

public sealed class PrevFinanceDbContext : DbContext
{
    private readonly ICurrentUserContext? _currentUserContext;
    private Guid? CurrentUserId => _currentUserContext?.UserId;

    public PrevFinanceDbContext(DbContextOptions<PrevFinanceDbContext> options, ICurrentUserContext? currentUserContext = null) : base(options)
    {
        _currentUserContext = currentUserContext;
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<AccountBalanceAdjustment> AccountBalanceAdjustments => Set<AccountBalanceAdjustment>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<InstallmentPlan> InstallmentPlans => Set<InstallmentPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrevFinanceDbContext).Assembly);

        modelBuilder.Entity<Profile>()
            .HasQueryFilter(x => !CurrentUserId.HasValue || x.UserId == CurrentUserId.Value);

        modelBuilder.Entity<Account>()
            .HasQueryFilter(x => !CurrentUserId.HasValue || x.UserId == CurrentUserId.Value);

        modelBuilder.Entity<AccountBalanceAdjustment>()
            .HasQueryFilter(x => !CurrentUserId.HasValue || x.UserId == CurrentUserId.Value);

        modelBuilder.Entity<Transaction>()
            .HasQueryFilter(x => !CurrentUserId.HasValue || x.UserId == CurrentUserId.Value);

        modelBuilder.Entity<InstallmentPlan>()
            .HasQueryFilter(x => !CurrentUserId.HasValue || x.UserId == CurrentUserId.Value);

        modelBuilder.Entity<RefreshToken>()
            .HasQueryFilter(x => !CurrentUserId.HasValue || x.UserId == CurrentUserId.Value);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyAuditData();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditData();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditData()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(utcNow);
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkAsUpdated(utcNow);
            }
        }
    }
}
